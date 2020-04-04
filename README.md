# C# Excel Query

C# library for extra general purpose functionality not found in the standard libraries.

## How to Use

*See the included example project to see it in action*

## Logging

The library supports injecting a logger object to be used to log issues using the API.
To avoid dependencies on any particular logging library, the logger instance used should implement the `CsharpExtras.Log.ILogger` interface.

To inject a logger:

```csharp
ICsharpExtrasApi api = new CsharpExtrasApiImpl();
ILogger logger = // ...
api.SetLogger(logger);
```

The logger can be set to `null` to disable logging.
