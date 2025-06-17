using System.IO.Compression;
using System.Text;
using INIManager.Components.Models;
using INIManager.Components.Services.Interfaces;

namespace INIManager.Components.Services;

public class ExportService : IExportService
{
    private readonly ConfigurationService _configurationService;
    private readonly ConfigurationDraftService _configurationDraftService;
    private readonly IConfiguration _configuration;

    public ExportService(ConfigurationService configurationService, ConfigurationDraftService configurationDraftService, IConfiguration configuration)
    {
        _configurationService = configurationService;
        _configurationDraftService = configurationDraftService;
        _configuration = configuration;
    }

    public async Task<byte[]> ExportConfigurationAsync(Configuration config)
    {
        if (config == null || string.IsNullOrWhiteSpace(config.Bezeichnung))
            throw new Exception("Konfiguration ungültig.");

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
                sb.AppendLine($"[Workstation {ws.Sequence}]");
                sb.AppendLine($"   Name = {ws.Name}");
                sb.AppendLine($"   Description = {ws.Description}");
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