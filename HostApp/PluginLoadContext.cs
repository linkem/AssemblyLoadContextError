using System.Reflection;
using System.Runtime.Loader;

namespace PluginHost;

// code from https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support#load-plugins
public class PluginLoadContext : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver;

    public PluginLoadContext(string pluginPath, string name) : base(name)
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        if (FrameworkAssemblies.IsFrameworkAssembly(assemblyName))
        {
            // framework assemblies should be resolved by Default context
            try
            {
                var defaultAssembly = Default.LoadFromAssemblyName(assemblyName);
                return defaultAssembly;
            }
            catch (Exception)
            {
                return null;
            }
        }

        string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}