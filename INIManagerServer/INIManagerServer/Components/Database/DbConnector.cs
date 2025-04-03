using MySqlConnector;

namespace INIManagerServer.Components.Database;

public class DbConnector : IDisposable
{
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
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
        await _connectionLock.WaitAsync(); // Warten, falls bereits eine Verbindung geöffnet wird
        try
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }
        }
        finally
        {
            _connectionLock.Release(); // Sperre freigeben
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