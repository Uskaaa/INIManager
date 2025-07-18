﻿using MySqlConnector;

namespace INIManager.Components.Database;

public class DbConnector
{
    private readonly string _connectionString;

    public DbConnector(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    // Gibt eine neue, bereits geöffnete Verbindung zurück
    public async Task<MySqlConnection> OpenConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}