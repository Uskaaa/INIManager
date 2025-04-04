using System.Text.Json;
using Microsoft.JSInterop;

namespace INIManagerServer.Components.Services;

public class Callbacker
{
    private IJSRuntime _js;
    private DotNetObjectReference<Callbacker> _this;
    private Dictionary<string, Action<string[]>> _callbacks = new();

    public Callbacker(IJSRuntime JSRuntime)
    {
        _js = JSRuntime;
        _this = DotNetObjectReference.Create(this);
    }

    [JSInvokable]
    public void _Callback(string callbackId, string[] arguments)
    {
        if (_callbacks.TryGetValue(callbackId, out Action<string[]> callback))
        {
            _callbacks.Remove(callbackId);
            callback(arguments);
        }
    }

    public Task<CallbackerResponse> InvokeJS(string cmd, params object[] args)
    {
        var t = new TaskCompletionSource<CallbackerResponse>();
        _InvokeJS((string[] arguments) => {
            t.TrySetResult(new CallbackerResponse(arguments));
        }, cmd, args);
        return t.Task;
    }

    public void InvokeJS(Action<CallbackerResponse> callback, string cmd, params object[] args)
    {
        _InvokeJS((string[] arguments) => {
            callback(new CallbackerResponse(arguments));
        }, cmd, args);
    }

    private void _InvokeJS(Action<string[]> callback, string cmd, object[] args)
    {
        string callbackId;
        do
        {
            callbackId = Guid.NewGuid().ToString();
        } while (_callbacks.ContainsKey(callbackId));
        _callbacks[callbackId] = callback;
        _js.InvokeVoidAsync("window._callbacker", _this, "_Callback", callbackId, cmd, JsonSerializer.Serialize(args));
    }
}