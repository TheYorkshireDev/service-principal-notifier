# Service Principal Notifier

## Introduction
Service Principal Notifier is a project that monitors Service Principals (SP) and notifies via email when SPs are expiring or have expired. 

## 🛠 Development
### Prerequisites

At a minimum, you will need the following installed and configured in order to run this project

* [.NET CLI](https://dotnet.microsoft.com/download) (`dotnet`)
* An Azure Storage Emulator such as [Azurite](https://github.com/Azure/Azurite)

### Building and Running

**Format**
```sh
dotnet format
```

**Build**
```sh
dotnet build
```

**Run**
```sh
# Ensure an Azure storage emulator is running

cd ServicePrincipalNotifier/
func start
```
