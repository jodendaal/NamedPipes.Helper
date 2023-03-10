[![Build](https://github.com/jodendaal/NamedPipes.Helper/actions/workflows/main.yml/badge.svg)](https://github.com/jodendaal/NamedPipes.Helper/actions/workflows/main.yml)
[![NuGet Badge](https://buildstats.info/nuget/NamedPipes.Helper)](https://www.nuget.org/packages/NamedPipes.Helper)

# Named Pipes Helper 
Simple library to work with named pipes for IPC communication. 

Also includes a helper to assist with creating single instance applications and forwarding messages between them.

See example code [Here](https://github.com/jodendaal/NamedPipes.Helper/tree/main/Examples)

# Getting started

Install package [Nuget package](https://www.nuget.org/packages/NamedPipes.Helper)

```powershell
Install-Package NamedPipes.Helper
```

# Server
Listen for messages on pipe named 'MyPipeName' with 2 threads

```csharp
_server = new Server("MyPipeName", (mesage) =>
{
    Console.WriteLine(message);
});

_server.Start(2);
```

## Client
Publish message to pipe named 'MyPipeName'

#### Async
```csharp
await Client.SendMessageAsync("MyPipeName","a test message");
```

#### Synchronise
```csharp
Client.SendMessage("MyPipeName","a test message");
```

## Single Instance Application

### Only allow single instance of app
```csharp
internal static class Program
{
    static Form1? _form;
    
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        App.SingleInstance("MyAppId", () =>
        {
            _form = new Form1();
            Application.Run(_form);

        });
    }
}
```


### Only allow single instance of app and send message to already running instance

```csharp
internal static class Program
{
    static Form1? _form;
    
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        App.SingleInstance("MyAppId", () =>
        {
            _form = new Form1();
            Application.Run(_form);

        },(mesage)=> {
            _form?.Invoke(()=> _form.ShowMessage(mesage));
        }, args.Length> 0 ?
        $"Another instance started with args : {string.Join(" ", args)}" 
        : "Another instance started with no args");
    }
}
```