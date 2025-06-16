using INIManager.Components.Models;

namespace INIManager.Components.Services.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportConfigurationAsync(Configuration config);
}