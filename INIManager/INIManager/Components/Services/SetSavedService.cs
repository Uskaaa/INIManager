using System.Threading.Channels;
using INIManager.Components.Models;

namespace INIManager.Components.Services;

public class SetSavedService
{
    private readonly Channel<List<Workstation>> _channel = Channel.CreateUnbounded<List<Workstation>>();

    public async Task PublishAsync(List<Workstation> workstations)
    {
        foreach (var workstation in workstations)
        {
            workstation.IsSaved = true;
        }
        await _channel.Writer.WriteAsync(workstations);
    }

    public async Task<List<Workstation>?> ListenOnceAsync(CancellationToken token = default)
    {
        return await _channel.Reader.ReadAsync(token);
    }
}