namespace App.Core.Plugins
{
    public interface ICorePlugin
    {
        IShellPlugin Shell { get; }
    }
}