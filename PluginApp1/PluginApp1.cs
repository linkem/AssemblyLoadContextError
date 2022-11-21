using Plugin.Abstraction;
using System.Net.Http.Json;
using System.Text.Json;

namespace PluginApp1;
public class PluginApp1 : IPlugin
{
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        // get HttpContentJsonExtensions.ReadFromJsonAsync<T>(this HttpContent content, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        var methodInfo = typeof(HttpContentJsonExtensions)
            .GetMethods()
            .Where(x => x.Name == "ReadFromJsonAsync" 
                && x.IsPublic 
                && x.IsPublic 
                && x.IsGenericMethod 
                && x.GetParameters().Any(s => s.Name == "options"))
            .First();
        var optionsArgumentAssembly = methodInfo.GetParameters().First(x => x.Name == "options").ParameterType.Assembly;

        // compare JsonSerializerOptions from `typeof(JsonSerializerOptions).Assembly` and from `HttpContentJsonExtensions.ReadFromJsonAsync` method
        if (typeof(JsonSerializerOptions).Assembly != optionsArgumentAssembly)
        {
            Console.WriteLine("Assembly mismatch!");
        }

        // uncommont to throw `System.MissingMethodException: Method not found...`
        // to make it work => change `System.Text.Json` package to version `6.0.0`
        //var response = await new HttpClient().GetAsync("https://jsonplaceholder.typicode.com/todos/1");
        //var jsonContent = response.Content.ReadFromJsonAsync<TodoModel>();

        PrintDuplicatedAssemblies();
    }

    private void PrintDuplicatedAssemblies()
    {
        var ass = AppDomain.CurrentDomain
            .GetAssemblies().ToList();
        var duplicatedAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .GroupBy(x => x.FullName, (key, assemblies) =>
                new
                {
                    AssemblyName = key,
                    Assemblies = assemblies
                })
            .Where(x => x.Assemblies.Count() > 1)
            .ToList();

        foreach (var duplicatedAssembly in duplicatedAssemblies)
        {
            Console.WriteLine($"Assembly: {duplicatedAssembly.AssemblyName}");
            foreach (var assembly in duplicatedAssembly.Assemblies)
            {
                Console.WriteLine($"{assembly}; Location: {assembly.Location}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}

public record TodoModel
{
    public int UserId { get; set; }
}
