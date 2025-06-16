using INIManager.Components.Models;

namespace INIManager.Components.Services;

public interface IWorkstationService
{
    Task<bool> CreateWorkstation(Workstation workstation);
    Task<List<Workstation>> ReadWorkstation();
}