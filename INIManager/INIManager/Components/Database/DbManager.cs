using INIManager.Components.Models;
using MySqlConnector;

namespace INIManager.Components.Database;

public class DbManager
{
    private readonly DbConnector _dbConnector;

    public DbManager(DbConnector dbConnector)
    {
        _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));
    }

    // Beispiel: Tabelle für Items erstellen
    public async Task CreateItemsTableAsync()
    {
        await _dbConnector.OpenConnectionAsync();
        using var command = new MySqlCommand(
            "CREATE TABLE IF NOT EXISTS items (" +
            "id VARCHAR(50) PRIMARY KEY, " +
            "name VARCHAR(100) NOT NULL, " +
            "zone_id VARCHAR(50) NOT NULL);",
            _dbConnector.GetConnection());
        await command.ExecuteNonQueryAsync();
        await _dbConnector.CloseConnectionAsync();
    }

    // Beispiel: Item einfügen oder aktualisieren
    public async Task UpsertItemAsync(string id, string name, string zoneId)
    {
        await _dbConnector.OpenConnectionAsync();
        using var command = new MySqlCommand(
            "INSERT INTO items (id, name, zone_id) VALUES (@id, @name, @zoneId) " +
            "ON DUPLICATE KEY UPDATE name = @name, zone_id = @zoneId;",
            _dbConnector.GetConnection());
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@zoneId", zoneId);
        await command.ExecuteNonQueryAsync();
        await _dbConnector.CloseConnectionAsync();
    }

    // Beispiel: Alle Items abrufen
    public async Task<List<DropItem>> GetAllItemsAsync()
    {
        var items = new List<DropItem>();
        await _dbConnector.OpenConnectionAsync();
        using var command = new MySqlCommand("SELECT id, name, zone_id FROM items;", _dbConnector.GetConnection());
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            items.Add(new DropItem
            {
                Id = reader.GetString("id"),
                Name = reader.GetString("name"),
                ZoneId = reader.GetString("zone_id")
            });
        }
        await _dbConnector.CloseConnectionAsync();
        return items;
    }
    
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            await _dbConnector.OpenConnectionAsync();
            await _dbConnector.CloseConnectionAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
            return false;
        }
    }
}