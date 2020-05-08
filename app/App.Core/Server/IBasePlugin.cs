namespace App.Core.Server
{
    public interface IBasePlugin
    {
        IBrowserPlugin Browser { get; }
        IResourcePlugin Resource { get; }
        ISocketPlugin Socket { get; }
        ITracePlugin Trace { get; }
    }
}