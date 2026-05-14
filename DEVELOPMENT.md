# Development Guide

This page contains the steps to build and run the Syncfusion Toolkit for Blazor repository from source. If you are looking to build apps with the Syncfusion Toolkit for Blazor, please head over to the links in the [README](https://github.com/syncfusion/blazor-toolkit/blob/main/README.md) to get started.

## Initial setup
   ### Windows
   - Install Visual Studio 2022 (v17.10 or newer) with ASP.NET and web development workload.
   - Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or latest version .NET SDK.
   - Clone the repository:
     ```shell
     git clone https://github.com/syncfusion/blazor-toolkit.git
     cd blazor-toolkit
     ```

## Building the Solution

1. Open a command prompt/terminal window.
2. Navigate to the location of your cloned `syncfusion/blazor-toolkit` repo.
3. Run these commands:
    ```dotnetcli
    dotnet restore
    dotnet build ./Syncfusion.Blazor.Toolkit.slnx
    ```

## Running Samples

- Open the `samples/Blazor.Toolkit.Samples.slnx` file in Visual Studio.
- Set the desired sample project as the startup project and run.

## What branch should I use?

As a general rule:
- [main](https://github.com/syncfusion/blazor-toolkit/tree/main)

## Sample projects

### Samples
```
├── samples
│   ├── Blazor.Toolkit.Samples
│   ├── Blazor.Toolkit.Samples.Client
```

- *Blazor.Toolkit.Samples*: Full gallery sample with all of the controls and features of the Syncfusion Blazor Toolkit.
- *Blazor.Toolkit.Samples.Client*: Client-side Blazor WebAssembly sample project.
