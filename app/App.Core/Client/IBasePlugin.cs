namespace App.Core.Client
{
    public interface IBasePlugin
    {
        IProxyPlugin Proxy { get; }
        IShellPlugin Shell { get; }
    }
}