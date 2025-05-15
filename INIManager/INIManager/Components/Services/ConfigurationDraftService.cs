using INIManager.Components.Database;
using INIManager.Components.Models;
using INIManager.Components.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MySqlConnector;

namespace INIManager.Components.Services;

public class ConfigurationDraftService : IConfigurationService
{
    private readonly DbConnector _dbConnector;
    private readonly IServiceProvider _serviceProvider;

    public ConfigurationDraftService(DbConnector dbConnector, IServiceProvider serviceProvider)
    {
        _dbConnector = dbConnector;
        _serviceProvider = serviceProvider;
    }

    public async Task<bool> CreateConfiguration(Configuration configuration)
    {
        try
        {
            await using var connection = await _dbConnector.OpenConnectionAsync();
            await using var command = new MySqlCommand(
                "INSERT INTO configuration_draft (id, bezeichnung, timestamp) VALUES (@id, @bezeichnung, @timestamp) ON DUPLICATE KEY UPDATE bezeichnung = @bezeichnung, timestamp = @timestamp;",
                connection);
            command.Parameters.AddWithValue("@id", configuration.Id);
            command.Parameters.AddWithValue("@bezeichnung", configuration.Bezeichnung);
            command.Parameters.AddWithValue("@timestamp", configuration.Timestamp);
            await command.ExecuteNonQueryAsync();

            if (configuration.Workstations != null)
            {
                configuration.Workstations.RemoveAll(x => x.IsSaved);
                foreach (var workstation in configuration.Workstations)
                {
                    await using var command3 = new MySqlCommand(
                        "INSERT INTO configws (configurationdraftid, workstationid, sequence) VALUES (@configurationdraftid, @workstationid, @sequence);",
                        connection);
                    command3.Parameters.AddWithValue("@configurationdraftid", configuration.Id);
                    command3.Parameters.AddWithValue("@workstationid", workstation.Id);
                    command3.Parameters.AddWithValue("@sequence", workstation.Sequence);
                    await command3.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        Console.WriteLine("Erstellt!");
        var hubContext = _serviceProvider.GetRequiredService<IHubContext<ConfigurationHub>>();
        await hubContext.Clients.All.SendAsync("ReloadConfigurations");
        return true;
    }

    public async Task<List<Configuration>> ReadConfiguration()
    {
        var configurations = new List<Configuration>();
        
        await using var connection = await _dbConnector.OpenConnectionAsync();
        await using (var command =
                     new MySqlCommand(
                         "SELECT id, bezeichnung, timestamp " +
                         "FROM configuration_draft;",
                         connection))
        {
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                configurations.Add(new Configuration
                {
                    Id = reader.GetInt32("id"),
                    Bezeichnung = reader.IsDBNull(reader.GetOrdinal("bezeichnung"))
                        ? string.Empty
                        : reader.GetString("bezeichnung"),
                    Timestamp = reader.IsDBNull(reader.GetOrdinal("timestamp"))
                        ? string.Empty
                        : reader.GetString("timestamp")
                });
            }
        }

        var workstations = new List<Workstation>();
        await using (var command2 = new MySqlCommand(
                         "SELECT workstation.id, workstation.name, workstation.description, configws.sequence FROM configws " +
                         "INNER JOIN workstation ON workstation.id = configws.workstationid " +
                         "WHERE configws.configurationdraftid = @configId;",
                         connection))
        {
            foreach (var configuration in configurations)
            {
                workstations = new List<Workstation>();
                command2.Parameters.Clear();
                command2.Parameters.AddWithValue("@configId", configuration.Id);
                await using var reader2 = await command2.ExecuteReaderAsync();
                while (await reader2.ReadAsync())
                {
                    workstations.Add(new Workstation
                    {
                        Id = reader2.GetInt32("id"),
                        Name = reader2.GetString("name"),
                        Description = reader2.GetString("description"),
                        Sequence = reader2.GetInt32("sequence")
                    });
                }

                configuration.Workstations = workstations;
            }
        }

        return configurations;
    }

    public async Task<Configuration> ReadConfigurationById(int id)
    {
        var configuration = new Configuration();

        await using var connection = await _dbConnector.OpenConnectionAsync();
        Console.WriteLine($"Connection State: {_dbConnector.GetConnection().State}");

        await using (var command = new MySqlCommand(
                         "SELECT id, bezeichnung, timestamp " +
                         "FROM configuration_draft " +
                         "WHERE id = @configId;",
                         connection))
        {
            command.Parameters.AddWithValue("@configId", id);
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                configuration.Id = reader.GetInt32("id");
                configuration.Bezeichnung = reader.IsDBNull(reader.GetOrdinal("bezeichnung"))
                    ? string.Empty
                    : reader.GetString("bezeichnung");
                configuration.Timestamp = reader.IsDBNull(reader.GetOrdinal("timestamp"))
                    ? string.Empty
                    : reader.GetString("timestamp");
            }
            else
            {
                Console.WriteLine("Keine Daten gefunden!");
            }
        }

        await using (var command2 = new MySqlCommand(
                         "SELECT workstation.id, workstation.name, workstation.description, configws.sequence FROM configws " +
                         "INNER JOIN workstation ON workstation.id = configws.workstationid " +
                         "WHERE configws.configurationdraftid = @configId;",
                         connection))
        {
            command2.Parameters.AddWithValue("@configId", id);
            await using var reader2 = await command2.ExecuteReaderAsync();
            var workstations = new List<Workstation>();

            while (await reader2.ReadAsync())
            {
                workstations.Add(new Workstation
                {
                    Id = reader2.GetInt32("id"),
                    Name = reader2.GetString("name"),
                    Description = reader2.GetString("description"),
                    Sequence = reader2.GetInt32("sequence")
                });
            }

            configuration.Workstations = workstations;
        }

        return configuration;
    }

    public async Task<bool> DeleteConfiguration(int id)
    {
        try
        {
            await using var connection = await _dbConnector.OpenConnectionAsync();
            using var command = new MySqlCommand(
                "DELETE FROM configws WHERE configurationdraftid = @id;",
                connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
            
            using var command2 = new MySqlCommand(
                "DELETE FROM configuration_draft WHERE id = @id;",
                connection);
            command2.Parameters.AddWithValue("@id", id);
            await command2.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        Console.WriteLine("Deleted!");
        var hubContext = _serviceProvider.GetRequiredService<IHubContext<ConfigurationHub>>();
        await hubContext.Clients.All.SendAsync("ReloadConfigurations");
        return true;
    }

    public async Task<bool> DeleteWorkstationsOfConfiguration(int configurationid,
        List<Workstation> workstationsToDelete)
    {
        try
        {
            await using var connection = await _dbConnector.OpenConnectionAsync();
            foreach (var workstationToDelete in workstationsToDelete)
            {
                using var command = new MySqlCommand(
                    "DELETE FROM configws WHERE configurationdraftid = @configurationId AND workstationid = @workstationId;",
                    connection);
                command.Parameters.AddWithValue("@configurationId", configurationid);
                command.Parameters.AddWithValue("@workstationId", workstationToDelete.Id);
                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        Console.WriteLine("Deleted!");
        var hubContext = _serviceProvider.GetRequiredService<IHubContext<ConfigurationHub>>();
        await hubContext.Clients.All.SendAsync("ReloadConfigurations");
        return true;
    }
}