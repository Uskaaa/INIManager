using System.Threading.Channels;
using INIManagerServer.Components.Models;

namespace INIManagerServer.Components.Services;

public class SetSavedService
{
    private readonly Channel<string> _channel = Channel.CreateUnbounded<string>();

    public async Task PublishAsync(string message)
    {
        await _channel.Writer.WriteAsync(message);
    }

    public async Task<string?> ListenOnceAsync(CancellationToken token = default)
    {
        return await _channel.Reader.ReadAsync(token);
    }
}