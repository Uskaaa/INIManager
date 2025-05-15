using INIManager.Components.Database;
using INIManager.Components.Models;
using MySqlConnector;

namespace INIManager.Components.Services;

public class WorkstationService
{
    private readonly DbConnector _dbConnector;

    public WorkstationService(DbConnector dbConnector)
    {
        _dbConnector = dbConnector;
    }

    public async Task<List<Workstation>> ReadWorkstation()
    {
        var workstations = new List<Workstation>();

        await using var connection = await _dbConnector.OpenConnectionAsync();
        await using (var command = new MySqlCommand(
                         "SELECT id, name, description " +
                         "FROM workstation;",
                         connection))
        {
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                workstations.Add(new Workstation
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Description = reader.GetString("description"),
                });
            }
        }
        return workstations;
    }

    public async Task<Workstation> ReadWorkstationById(int id)
    {
        var workstation = new Workstation();

        await using var connection = await _dbConnector.OpenConnectionAsync();
        await using (var command =
                     new MySqlCommand(
                         "SELECT id, name, description " +
                         "FROM workstation" +
                         "WHERE id = @configId;",
                         connection))
        {
            command.Parameters.AddWithValue("@configId", id);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                workstation = new Workstation
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Description = reader.GetString("description")
                };
            }
        }

        return workstation;
    }

    public async Task<bool> UpdateWorkstation(Workstation workstation)
    {
        return true;
    }

    public async Task<bool> DeleteWorkstation(int id)
    {
        return true;
    }
}