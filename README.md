# This library provides [Starcounter Authorization Library](https://github.com/Starcounter/authorization) with support for [Simplified Data Model](https://github.com/StarcounterApps/Simplified)

## Building and testing

```
nuget restore
msbuild Authorization.Simplified.sln
.\packages\xunit.runner.console.2.3.1\tools\net452\xunit.console.exe .\Authorization.Simplified.Tests\bin\Debug\Authorization.Database.Tests.dll
```
