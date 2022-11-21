# Overview
Repository presents problem with `AssemblyLoadContext` when loading `PluginApp`

* `Plugin.Abstraction` - abstraction for plugin
* `PluginHost` - host of plugin, uses `AssemblyLoadContext` to load assemblies
* `PluginApp1` - actual plugin app

# Steps to reproduce
## Repro with exception
* Build `PluginApp1` application
* Start `PluginHost`
* Exception is thrown:
    ```
    System.MissingMethodException
        HResult=0x80131513
        Message=Method not found: 'System.Threading.Tasks.Task`1<!!0> System.Net.Http.Json.HttpContentJsonExtensions.ReadFromJsonAsync(System.Net.Http.HttpContent, System.Text.Json.JsonSerializerOptions, System.Threading.CancellationToken)'.
        Source=PluginApp1
        StackTrace:
            at PluginApp1.PluginApp1.<Execute>d__0.MoveNext() in C:\projects\GitHub\Github.AssemblyLoadContextError\PluginApp1\PluginApp1.cs:line 32
            at System.Runtime.CompilerServices.AsyncMethodBuilderCore.Start[TStateMachine](TStateMachine& stateMachine) in /_/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/AsyncMethodBuilderCore.cs:line 38
            at System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start[TStateMachine](TStateMachine& stateMachine) in /_/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/AsyncTaskMethodBuilder.cs:line 33
            at PluginApp1.PluginApp1.Execute(CancellationToken cancellationToken) in C:\projects\GitHub\Github.AssemblyLoadContextError\PluginApp1\PluginApp1.cs:line 9
            at Program.<<Main>$>d__0.MoveNext() in C:\projects\GitHub\Github.AssemblyLoadContextError\HostApp\Program.cs:line 12
            at Program.<Main>(String[] args)
    ```

## Repro with listed duplicated assemblies
* Comment out `PluginApp1.PluginApp1.cs` line `30`
* Build `PluginApp1` application
* Start `PluginHost`
* Application should print output similiar to this one:
    ```
    For Assembly 'System.Text.Json' found multiple instances:

    System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51; Location: <PROJECT_PATH>\PluginApp1\bin\Debug\net6.0\System.Text.Json.dll

    System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51; Location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.11\System.Text.Json.dll
    ```
* Output indicates that there were loaded multiple assemblies with same name 'System.Text.Json'



# Workaround
Use `System.Text.Json` with version `6.0.0` - not sufficient for more complicated scenarios

# Expected output
Application load only one `System.Text.Json` and exception is not thrown.


