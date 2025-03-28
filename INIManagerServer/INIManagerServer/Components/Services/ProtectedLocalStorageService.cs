using INIManagerServer.Components.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace INIManagerServer.Components.Services;

public class ProtectedLocalStorageService
{
    ProtectedLocalStorage _protectedLocalStorage;
    
    public ProtectedLocalStorageService(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }
    
    public async Task AddItemAsync(string key, Settings settings)
    {
        try
        {
            await _protectedLocalStorage.SetAsync(key, settings);
        }
        catch (Exception ex)
        {
            // Logge den Fehler oder wirf eine benutzerdefinierte Exception
            throw new InvalidOperationException($"Failed to add item to ProtectedLocalStorage: {ex.Message}", ex);
        }
    }

    public async Task<Settings> GetItemAsync(string key)
    {
        try
        {
            var result = await _protectedLocalStorage.GetAsync<Settings>(key);
            return result.Value;
        }
        catch (Exception ex)
        {
            // Loggen oder Standardwert zurückgeben
            throw new InvalidOperationException($"Failed to get item from ProtectedLocalStorage: {ex.Message}", ex);
        }
    }
}