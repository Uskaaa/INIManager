using INIManager.Components.Database;
using INIManager.Components.Models;
using MySqlConnector;

namespace INIManager.Components.Services;

public class WorkstationService : IWorkstationService
{
    private readonly DbConnector _dbConnector;

    public WorkstationService(DbConnector dbConnector)
    {
        _dbConnector = dbConnector;
    }

    public async Task<bool> CreateWorkstation(Workstation workstation)
    {
        try
        {
            await using var connection = await _dbConnector.OpenConnectionAsync();
            
            await using var command = new MySqlCommand(
                "INSERT INTO workstation (id, bezeichnung, typeid, minguisampletimeinms, highactive, tcadsdiaddress, tcadsdiadsport) VALUES (@id, @bezeichnung, @typeid, @minguisampletimeinms, @highactive, @tcadsdiaddress, @tcadsdiadsport) " +
                "ON DUPLICATE KEY UPDATE bezeichnung = @bezeichnung, typeid = @typeid, minguisampletimeinms = @minguisampletimeinms, highactive = @highactive, tcadsdiaddress = @tcadsdiaddress, tcadsdiadsport = @tcadsdiadsport;",
                connection);
            command.Parameters.AddWithValue("@id", workstation.Id);
            command.Parameters.AddWithValue("@bezeichnung", workstation.Bezeichnung);
            command.Parameters.AddWithValue("@typeid", workstation.WorkstationType.TypeId);
            command.Parameters.AddWithValue("@minguisampletimeinms", workstation.MinGuiSampleTimeInMs);
            command.Parameters.AddWithValue("@highactive", workstation.HighActive);
            command.Parameters.AddWithValue("@tcadsdiaddress", workstation.TcAdsDiAddress);
            command.Parameters.AddWithValue("@tcadsdiadsport", workstation.TcAdsDiAdsPort);
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        return true;
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
                    Bezeichnung = reader.GetString("name"),
                    Description = reader.GetString("description"),
                });
            }
        }

        return workstations;
    }
}