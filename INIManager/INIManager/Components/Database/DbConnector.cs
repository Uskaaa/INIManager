using MySqlConnector;

namespace INIManager.Components.Database;

public class DbConnector : IDisposable
{
    private readonly string _connectionString;
    private MySqlConnection _connection;

    public DbConnector(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _connection = new MySqlConnection(_connectionString);
    }

    // Verbindung öffnen
    public async Task OpenConnectionAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }

    // Verbindung schließen
    public async Task CloseConnectionAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            await _connection.CloseAsync();
        }
    }

    // Zugriff auf die Verbindung
    public MySqlConnection GetConnection()
    {
        return _connection;
    }

    // IDisposable-Implementierung für saubere Ressourcenfreigabe
    public void Dispose()
    {
        _connection?.Dispose();
    }
}