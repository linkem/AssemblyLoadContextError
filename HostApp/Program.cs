using Plugin.Abstraction;
using PluginHost;
using System.Reflection;
//Console.WriteLine($"Current dir: {Directory.GetCurrentDirectory()}");
var absolutePathToPluginApp1 = Path.GetFullPath(@"..\PluginApp1\bin\Debug\net6.0\PluginApp1.dll");

var assembly = (new PluginLoadContext(absolutePathToPluginApp1, "PluginApp1Context"))
    .LoadFromAssemblyPath(absolutePathToPluginApp1);

var appInstance = CreateAppInstance(assembly);
await appInstance.Execute();

Console.WriteLine("Loader Exit.");
Console.ReadKey();
static IPlugin CreateAppInstance(Assembly assembly)
{
    return assembly.GetTypes()
        .Where(type => typeof(IPlugin).IsAssignableFrom(type))
        .Select(type => Activator.CreateInstance(type, true) as IPlugin)
        .Where(instance => instance != null)
        .OfType<IPlugin>()
        .Single();
}