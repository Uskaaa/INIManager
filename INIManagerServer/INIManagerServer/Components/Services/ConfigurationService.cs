using INIManagerServer.Components.Database;
using INIManagerServer.Components.Models;
using INIManagerServer.Components.Services.Interfaces;
using MySqlConnector;

namespace INIManagerServer.Components.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly DbConnector _dbConnector;

    public ConfigurationService(DbConnector dbConnector)
    {
        _dbConnector = dbConnector;
    }

    public async Task<bool> CreateConfiguration(Configuration configuration)
    {
        try
        {
            await _dbConnector.OpenConnectionAsync();
            using var command = new MySqlCommand(
                "INSERT INTO configuration (id, bezeichnung, timestamp) VALUES (@id, @name, @timestamp);",
                _dbConnector.GetConnection());
            command.Parameters.AddWithValue("@id", configuration.Id);
            command.Parameters.AddWithValue("@bezeichnung", configuration.Bezeichnung);
            command.Parameters.AddWithValue("@timestamp", configuration.Timestamp);
            await command.ExecuteNonQueryAsync();
            await _dbConnector.CloseConnectionAsync();
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
                         "FROM configuration;",
                         _dbConnector.GetConnection()))
        {
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var workstations = new List<Workstation>();
                await using (var command2 = new MySqlCommand(
                                 "SELECT workstation.id, workstation.bezeichnung, workstation.description FROM configws " +
                                 "INNER JOIN workstation ON workstation.id = configws.workstationid " +
                                 "WHERE configws.configurationid = @configId;",
                                 _dbConnector.GetConnection()))
                {
                    command2.Parameters.AddWithValue("@configId", reader.GetInt32("id"));
                    await using var reader2 = await command2.ExecuteReaderAsync();
                    while (await reader2.ReadAsync())
                    {
                        workstations.Add(new Workstation
                        {
                            Id = reader2.GetInt32("id"),
                            Name = reader2.GetString("name"),
                            Description = reader2.GetString("description"),
                        });
                    }
                }

                configurations.Add(new Configuration
                {
                    Id = reader.GetInt32("id"),
                    Bezeichnung = reader.GetString("bezeichnung"),
                    Workstations = workstations,
                    Timestamp = reader.GetString("dateOfCreation")
                });
            }

        }
        await _dbConnector.CloseConnectionAsync();

        return configurations;
    }

    public async Task<Configuration> ReadConfigurationById(int id)
    {
        var configuration = new Configuration();

        await _dbConnector.OpenConnectionAsync();
        await using (var command =
                     new MySqlCommand(
                         "SELECT id, bezeichnung, timestamp " +
                         "FROM configuration " +
                         "WHERE id = @configId;",
                         _dbConnector.GetConnection()))
        {
            command.Parameters.AddWithValue("@configId", id);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var workstations = new List<Workstation>();
                await using (var command2 = new MySqlCommand(
                                 "SELECT workstation.id, workstation.name, workstation.description FROM configws " +
                                 "INNER JOIN workstation ON workstation.id = configws.workstationid " +
                                 "WHERE configws.configurationid = @configId;",
                                 _dbConnector.GetConnection()))
                {
                    command2.Parameters.AddWithValue("@configId", reader.GetInt32("id"));
                    await using var reader2 = await command2.ExecuteReaderAsync();
                    while (await reader2.ReadAsync())
                    {
                        workstations.Add(new Workstation
                        {
                            Id = reader2.GetInt32("id"),
                            Name = reader2.GetString("name"),
                            Description = reader2.GetString("description"),
                        });
                    }
                }

                configuration = new Configuration
                {
                    Id = reader.GetInt32("id"),
                    Bezeichnung = reader.GetString("bezeichnung"),
                    Workstations = workstations,
                    Timestamp = reader.GetString("dateOfCreation")
                };
            }

        }
        await _dbConnector.CloseConnectionAsync();

        return configuration;
    }

    public async Task<bool> UpdateConfiguration(Configuration configuration)
    {
        
        return true;
    }

    public async Task<bool> DeleteConfiguration(int id)
    {
        return true;
    }
}