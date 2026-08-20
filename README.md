# ediFabric Native API for Azure Functions

## 1. Overview
This example hosts [ediFabric Native](https://www.edifabric.com/edifabric-native.html) behind an Azure Functions HTTP API. It translates X12 EDI to JSON and back, validates transaction sets, and generates acknowledgments using the C# bindings from [edifabric-csharp-bindings](https://github.com/EdiFabric/edifabric-csharp-bindings).

The native library is a self-contained shared library. No EdiFabric.Api NuGet package (aka EdiNation InHouse) is required on the target machine beyond this isolated-process Azure Functions host.

> NOTE: The example is for .NET [isolated process function](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide).

## 2. Requirements
- [Visual Studio](https://visualstudio.microsoft.com/vs/) or the .NET 10 SDK.
- If you don't have an [Azure subscription](https://docs.microsoft.com/en-us/azure/guides/developer/azure-developer-guide#understanding-accounts-subscriptions-and-billing), create an [Azure free account](https://azure.microsoft.com/free/?ref=microsoft.com&utm_source=microsoft.com&utm_medium=docs&utm_campaign=visualstudio) before you begin.
- [Download Postman](https://www.postman.com/downloads/) - it's an application to consume/test your API.
- The native library for your platform:

| Platform | File |
| --- | --- |
| Windows | `edifabric-x12-tools.dll` |
| Linux | `edifabric-x12-tools.so` |
| macOS | `edifabric-x12-tools.dylib` |

[Download **ediFabric Native** Library](https://support.edifabric.com/hc/en-us/articles/37289848931869-Download)

Put the library in the repository root or in `EdiFabric.Api.Azure`, or set `Configuration.LibraryPath` / `EDIFABRIC_X12_LIB`. The project copies it next to the worker on build and publish when it is found in those folders.

Linux Azure Function Apps cannot load the Windows `.dll`. Before publishing to Linux, place `edifabric-x12-tools.so` (glibc x64) in the project or repository root so it is included in the deployment package. Windows Function Apps need `edifabric-x12-tools.dll` instead.

- X12 test file(s). If you don't have a test file, use one of ours - [X12 HIPAA](https://support.edifabric.com/hc/en-us/sections/360001487352-X12-HIPAA-Files-Templates), [X12](https://support.edifabric.com/hc/en-us/sections/360005274077-X12-Files-Templates).

## 3. License
Set `Configuration.ApiKey` to your serial. The free-plan serial is:

```
bd96a836feca45cb91c86ee65d281f52
```

The free plan authorizes with `set_serial` only. Tokens (`BlobCache`) are available for the Enterprise license.

## 4. Setup
Rebuild the solution. If there are any build errors, contact us at https://support.edifabric.com/hc/en-us/requests/new for assistance.

The C# bindings live in `EdiFabric.Api.Azure/Native` (`NativeMethods.cs` and `EdiFabricX12.cs`), copied from the [edifabric-csharp-bindings](https://github.com/EdiFabric/edifabric-csharp-bindings) repository.

By default the API uses the online spec service (`SetMap` with `"default": "<serial>"`). To use local JSON models instead, place a `map/map.json` next to the function project (see the bindings README for the map format).

## 5. Getting started
Run the function app locally, then POST X12 EDI to:

| Endpoint | Native call | Input | Output
| --- | --- | --- | --- |
| `POST /x12/read` | `EdiFabricX12.Parse` (JSON only) | X12 | JSON |
| `POST /x12/write` | `EdiFabricX12.Build` | JSON (the output from /read) | X12 |
| `POST /x12/validate` | `EdiFabricX12.Parse` (JSON + validation report) | JSON (the output from /read) or X12 | JSON |
| `POST /x12/ack` | `EdiFabricX12.Parse` (JSON + validation + 999/997/TA1) | JSON (the output from /read) or X12 | JSON |

## 6. Warranty
*The source code in these example projects is strictly for demonstrational purposes and is provided "AS IS" without warranty of any kind, whether expressed or implied, including but not limited to the implied warranties of merchantability and/or fitness for a particular purpose.*

## 7. Additional information

[ediFabric Native documentation](https://support.edifabric.com/hc/en-us/articles/37276016388125-Introduction)

[C# bindings](https://github.com/EdiFabric/edifabric-csharp-bindings)

[Support](https://support.edifabric.com/hc/en-us/requests/new)
### 2026 © EdiFabric
