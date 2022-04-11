# C# Extras

This library contains general-purpose functionality built on top of the .NET Standard libraries. Much of the code was inspired writing Excel VSTO AddIns using the [Excel Interop](https://docs.microsoft.com/en-us/dotnet/api/microsoft.office.interop.excel?view=excel-pia) libraries. However, this code is intentionally abstracted away from such libraries and thus can be used anywhere that's compatible with the .NET Standard framework.

![.NET Core](https://github.com/ColmBhandal/CsharpExtras/workflows/.NET%20Core/badge.svg)

## Including C# Extras in Your Project

It is recommended to use NuGet to include C# Extras in your project. All releases and pre-releases are listed [here](https://github.com/ColmBhandal/CsharpExtras/releases); note: there are many v2 prereleases - this is because at the moment I do not plan to release v2.0.0 unless this library gets full-time support; at that point (if it happens) the plan would be to create milestones, do all the necessary refactoring, and release the next major version. More info on the versioning/release-strategy can be found on [this Wiki page](https://github.com/ColmBhandal/CsharpExtras/wiki/Versioning-and-Release-Strategy), although it's mostly just semantic versioning.

The NuGet page for CsharpExtras is [here](https://www.nuget.org/packages/CsharpExtras).

## How to Use

 - The code is documented in many places, but unfortunately documentation came a bit late so it is not comprehensive. There is a [TODO item](https://github.com/ColmBhandal/CsharpExtras/issues/126) for that.
 - There is an [example application](https://github.com/ColmBhandal/CsharpExtras/tree/develop/ExampleApplication) included in this repo that shows some of the code in action.
 - The Wiki has some [DotNetFiddle examples](https://github.com/ColmBhandal/CsharpExtras/wiki/DotNet-Fiddle-Examples) demonstrating the code in action - these allow you to interact with the C# Extras code in your browser.

## Contributing

See [Contributing.md](https://github.com/ColmBhandal/CsharpExtras/blob/develop/CONTRIBUTING.md)

## Logging

The library supports injecting a logger object to be used to log issues using the API.
To avoid dependencies on any particular logging library, the logger instance used should implement the `CsharpExtras.Log.ILogger` interface.

To inject a logger:

```csharp
CsharpExtrasApi api = new CsharpExtrasApiImpl();
ILogger logger = // ...
api.SetLogger(logger);
```

The logger can be set to `null` to disable logging.

## Redundancy & Bugs

Some of the code in this library may be redundant - there may be a more idiomatic way to achieve the same results in standard C#. Also, there may be performance issues or even bugs in the code. If you notice any such issues - feel free to submit an issue as per [Contributing Guidelines](https://github.com/ColmBhandal/CsharpExtras/blob/develop/CONTRIBUTING.md).
