using INIManager.Components.Models;

namespace INIManager.Components.Services.Interfaces;

public interface IAdoService
{
    Task<List<Workstation>> LoadWorkstationsFromAzureDevOps();
}