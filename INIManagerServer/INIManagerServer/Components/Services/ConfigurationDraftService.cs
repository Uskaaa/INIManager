using INIManagerServer.Components.Database;
using INIManagerServer.Components.Models;
using INIManagerServer.Components.Services.Interfaces;
using MySqlConnector;

namespace INIManagerServer.Components.Services;

public class ConfigurationDraftService : IConfigurationService
{
    private readonly DbConnector _dbConnector;

    public ConfigurationDraftService(DbConnector dbConnector)
    {
        _dbConnector = dbConnector;
    }

    public async Task<bool> CreateConfiguration(Configuration configuration)
    {
        try
        {
            await _dbConnector.OpenConnectionAsync();
            await using var command = new MySqlCommand(
                "INSERT INTO configuration_draft (id, bezeichnung, timestamp) VALUES (@id, @bezeichnung, @timestamp) ON DUPLICATE KEY UPDATE bezeichnung = @bezeichnung, timestamp = @timestamp;",
                _dbConnector.GetConnection());
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
                        _dbConnector.GetConnection());
                    command3.Parameters.AddWithValue("@configurationdraftid", configuration.Id);
                    command3.Parameters.AddWithValue("@workstationid", workstation.Id);
                    command3.Parameters.AddWithValue("@sequence", workstation.Sequence);
                    await command3.ExecuteNonQueryAsync();
                }

                await _dbConnector.CloseConnectionAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        Console.WriteLine("Erstellt!");
        return true;
    }

    public async Task<List<Configuration>> ReadConfiguration()
    {
        var configurations = new List<Configuration>();

        await _dbConnector.OpenConnectionAsync();
        await using (var command =
                     new MySqlCommand(
                         "SELECT id, bezeichnung, timestamp " +
                         "FROM configuration_draft;",
                         _dbConnector.GetConnection()))
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
                         _dbConnector.GetConnection()))
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

        await _dbConnector.CloseConnectionAsync();

        return configurations;
    }

    public async Task<Configuration> ReadConfigurationById(int id)
    {
        var configuration = new Configuration();

        await _dbConnector.OpenConnectionAsync();
        Console.WriteLine($"Connection State: {_dbConnector.GetConnection().State}");

        await using (var command = new MySqlCommand(
                         "SELECT id, bezeichnung, timestamp " +
                         "FROM configuration_draft " +
                         "WHERE id = @configId;",
                         _dbConnector.GetConnection()))
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
                         _dbConnector.GetConnection()))
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

        await _dbConnector.CloseConnectionAsync();

        return configuration;
    }

    public async Task<bool> DeleteConfiguration(int id)
    {
        try
        {
            await _dbConnector.OpenConnectionAsync();
            using var command = new MySqlCommand(
                "DELETE FROM configws WHERE configurationdraftid = @id;",
                _dbConnector.GetConnection());
            command.Parameters.AddWithValue("@configurationdraftid", id);
            await command.ExecuteNonQueryAsync();

            await _dbConnector.OpenConnectionAsync();
            using var command2 = new MySqlCommand(
                "DELETE FROM configuration WHERE id = @id;",
                _dbConnector.GetConnection());
            command2.Parameters.AddWithValue("@id", id);
            await command2.ExecuteNonQueryAsync();

            await _dbConnector.CloseConnectionAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        Console.WriteLine("Deleted!");
        return true;
    }

    public async Task<bool> DeleteWorkstationsOfConfiguration(int configurationid,
        List<Workstation> workstationsToDelete)
    {
        try
        {
            await _dbConnector.OpenConnectionAsync();
            foreach (var workstationToDelete in workstationsToDelete)
            {
                using var command = new MySqlCommand(
                    "DELETE FROM configws WHERE configurationid = @configurationId AND workstationid = @workstationId;",
                    _dbConnector.GetConnection());
                command.Parameters.AddWithValue("@configurationId", configurationid);
                command.Parameters.AddWithValue("@workstationId", workstationToDelete.Id);
                await command.ExecuteNonQueryAsync();
            }

            await _dbConnector.CloseConnectionAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        Console.WriteLine("Deleted!");
        return true;
    }
}