Located at 

Dahl.Data is a NuGet library that is added to your project to use ADO.NET in
a consistent manner.

Inherit 
Dahl.Data - simple facade for .NET
========================================
[![Build status](https://ci.appveyor.com/api/projects/status/8rbgoxqio76ynj4h?svg=true)](https://ci.appveyor.com/project/StackExchange/dapper)

Release Notes
-------------
Located at [kdahltx/Dahl.Data](https://github.com/kdahltx/Data.Data)

Packages
--------

MyGet Pre-release feed: https://www.myget.org/gallery/dapper

| Package | NuGet Stable | NuGet Pre-release | Downloads | MyGet |
| ------- | ------------ | ----------------- | --------- | ----- |
| [Dahl.Data](https://www.nuget.org/packages/Dahl.Data/) | [![Dahl.Data.Common](https://img.shields.io/nuget/Dahl.Data)](https://www.nuget.org/packages/Dapper/) | [![Dapper](https://img.shields.io/nuget/vpre/Dapper.svg)](https://www.nuget.org/packages/Dahl.Data/) | [![Dapper](https://img.shields.io/nuget/dt/Dahl.Data.svg)](https://www.nuget.org/packages/Dahl/) |


Features
--------
Dahl.Data is a [NuGet library](https://www.nuget.org/packages/Dahl.Data) that you can add in to your project to create a consistent framework for data access.

It provides 3 helpers:

Execute a query and map the results to a strongly typed List
------------------------------------------------------------
```csharp
public static IEnumerable<T> Query<T>(this IDbConnection cnn, string sql, object param = null, SqlTransaction transaction = null, bool buffered = true)
```

Example usage:
```csharp
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
}

var guid = Guid.NewGuid();
var dog = connection.Query<Dog>("select Age = @Age, Id = @Id", new { Age = (int?)null, Id = guid });

Assert.Equal(1,dog.Count());
Assert.Null(dog.First().Age);
Assert.Equal(guid, dog.First().Id);
```

