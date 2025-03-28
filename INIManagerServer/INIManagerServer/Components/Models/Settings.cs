using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace INIManagerServer.Components.Models;

public class Settings
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    
    public Settings(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }
    
    public bool SaveToProtectedStorage { get; set; }
    
}