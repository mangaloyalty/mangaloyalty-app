namespace App.Core.Server
{
    public interface IBasePlugin
    {
        IBrowserPlugin Browser { get; }
        IResourcePlugin Resource { get; }
        ITracePlugin Trace { get; }
    }
}