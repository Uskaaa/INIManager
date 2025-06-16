using System.IO.Compression;
using System.Text;
using INIManager.Components.Models;
using INIManager.Components.Services.Interfaces;

namespace INIManager.Components.Services;

public class ExportService : IExportService
{
    private readonly IConfigurationService _configurationService;
    private readonly IConfiguration _configuration;

    public ExportService(IConfigurationService configurationService, IConfiguration configuration)
    {
        _configurationService = configurationService;
        _configuration = configuration;
    }

    public async Task<byte[]> ExportConfigurationAsync(int configurationId)
    {
        var config = await _configurationService.ReadConfigurationById(configurationId);
        if (config == null || string.IsNullOrWhiteSpace(config.Bezeichnung))
            throw new InvalidOperationException("Konfiguration ungültig.");

        var sortedWorkstations = config.Workstations?
            .OrderBy(w => w.Sequence)
            .ToList() ?? new List<Workstation>();

        var exportFolderName = config.Bezeichnung;

        var templates = new Dictionary<string, string>
        {
            ["hardware.ini"] = _configuration["PreviewTexts:Hardware"] ?? "",
            ["params.ini"] = _configuration["PreviewTexts:Params"] ?? "",
            ["defaults.ini"] = _configuration["PreviewTexts:Default"] ?? "",
            ["m2ksys.ini"] = _configuration["PreviewTexts:M2kSys"] ?? ""
        };

        var files = new Dictionary<string, string>();

        foreach (var (filename, template) in templates)
        {
            var sb = new StringBuilder(template);

            for (int i = 0; i < sortedWorkstations.Count; i++)
            {
                var ws = sortedWorkstations[i];
                sb.AppendLine();
                sb.AppendLine($"[Workstation {i + 1}]");
                sb.AppendLine($"   Type = {ws.WorkstationType?.Bezeichnung}");
                sb.AppendLine($"   MinGuiSampleTimeInMs = {ws.MinGuiSampleTimeInMs}");
                sb.AppendLine($"   HighActive = {ws.HighActive}");
                sb.AppendLine($"   TcAdsDoAddress = {ws.TcAdsDiAddress}");
                sb.AppendLine($"   TcAdsDoAdsPort = {ws.TcAdsDiAdsPort}");
            }

            files[$"{exportFolderName}/{filename}"] = sb.ToString();
        }

        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var (path, content) in files)
            {
                var entry = archive.CreateEntry(path);
                using var entryStream = entry.Open();
                using var writer = new StreamWriter(entryStream);
                writer.Write(content);
            }
        }

        ms.Seek(0, SeekOrigin.Begin);
        return ms.ToArray();
    }
}