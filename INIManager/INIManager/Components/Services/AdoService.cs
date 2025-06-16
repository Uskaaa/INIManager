using INIManager.Components.Models;
using INIManager.Components.Services.Interfaces;
using System.Diagnostics;

namespace INIManager.Components.Services
{
    public class AdoService : IAdoService
    {
        private readonly string _repoUrl;
        private readonly string _personalAccessToken;
        private readonly string _tempClonePath;
        private readonly IWorkstationService _workstationService;

        public AdoService(string repoUrl, string pat, IWorkstationService workstationService,
            string tempClonePath = "temp_ado_clone")
        {
            _repoUrl = repoUrl;
            _personalAccessToken = pat;
            _workstationService = workstationService;
            _tempClonePath = tempClonePath;
        }

        public async Task<List<Workstation>> LoadWorkstationsFromAzureDevOps()
        {
            await CloneRepositoryAsync();
            string workstationPath = Path.Combine(_tempClonePath, "Automation", "Workstations");

            var workstations = await ParseIniFiles(workstationPath);

            foreach (var ws in workstations)
            {
                await _workstationService.CreateWorkstation(ws);
            }

            if (Directory.Exists(_tempClonePath))
                Directory.Delete(_tempClonePath, true);

            return workstations;
        }

        private async Task CloneRepositoryAsync()
        {
            if (Directory.Exists(_tempClonePath))
                Directory.Delete(_tempClonePath, true);

            string authRepoUrl = _repoUrl.Replace("https://", $"https://pat:{_personalAccessToken}@");

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"clone {authRepoUrl} {_tempClonePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
                throw new Exception($"Git clone failed: {error}");
        }

        private async Task<List<Workstation>> ParseIniFiles(string path)
        {
            var workstations = new List<Workstation>();
            var files = Directory.GetFiles(path, "*.ini");

            foreach (var file in files)
            {
                var config = new ConfigurationBuilder()
                    .AddIniFile(file)
                    .Build();

                var ws = new Workstation
                {
                    Id = int.Parse(config["Id"] ?? "0"),
                    Bezeichnung = config["Bezeichnung"] ?? "Unbekannt",
                    MinGuiSampleTimeInMs = float.Parse(config["MinGuiSampleTimeInMs"] ?? "0"),
                    HighActive = int.Parse(config["HighActive"] ?? "0"),
                    TcAdsDiAddress = config["TcAdsDiAddress"] ?? "",
                    TcAdsDiAdsPort = config["TcAdsDiAdsPort"] ?? "",
                    WorkstationType = new WorkstationType
                    {
                        TypeId = int.Parse(config["WorkstationType:TypeId"] ?? "0"),
                        Bezeichnung = config["WorkstationType:Bezeichnung"] ?? ""
                    }
                };

                workstations.Add(ws);
            }

            return workstations;
        }
    }
}