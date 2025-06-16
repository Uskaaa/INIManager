using INIManager.Components.Models;

namespace INIManager.Components.Services.Interfaces;

public interface IConfigurationService
{
    Task<bool> CreateConfiguration(Configuration configuration);
    Task<List<Configuration>> ReadConfiguration();
    Task<Configuration> ReadConfigurationById(int id);
    Task<bool> DeleteConfiguration(int id);
    Task<bool> DeleteWorkstationsOfConfiguration(int configurationid,
        List<Workstation> workstationsToDelete);
}