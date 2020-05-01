namespace App.Core.Plugins
{
    public interface ICorePlugin
    {
        IBrowserPlugin Browser { get; }
        IResourcePlugin Resource { get; }
        IShellPlugin Shell { get; }
    }
}