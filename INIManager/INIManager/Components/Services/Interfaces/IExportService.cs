namespace INIManager.Components.Services.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportConfigurationAsync(int configurationId);
}