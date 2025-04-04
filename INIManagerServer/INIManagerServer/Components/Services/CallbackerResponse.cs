using System.Text.Json;

namespace INIManagerServer.Components.Services;

public class CallbackerResponse
{
    public string[] arguments { get; private set; }
    public CallbackerResponse(string[] arguments)
    {
        this.arguments = arguments;
    }
    public T GetArg<T>(int i)
    {
        return JsonSerializer.Deserialize<T>(arguments[i]);
    }
}