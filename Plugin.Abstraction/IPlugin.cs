namespace Plugin.Abstraction;
public interface IPlugin
{
    Task Execute(CancellationToken cancellationToken = default);
}
