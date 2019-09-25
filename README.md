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
NuGet Release feed: https://www.nuget.org/


| Package | NuGet Stable | NuGet Pre-release | Downloads | MyGet |
| ------- | ------------ | ----------------- | --------- | ----- |
| [Dahl.Data](https://www.nuget.org/packages/Dahl.Data/) | [![Dahl.Data.Common](https://img.shields.io/nuget/Dahl.Data)](https://www.nuget.org/packages/Dahl.Data/) | [![Dapper](https://img.shields.io/nuget/vpre/Dapper.svg)](https://www.nuget.org/packages/Dahl.Data/) | [![Dapper](https://img.shields.io/nuget/dt/Dahl.Data.svg)](https://www.nuget.org/packages/Dahl.Data/) |


Features
--------
Dahl.Data is a [NuGet library](https://www.nuget.org/packages/Dahl.Data) that can be added to a project for creating a consistent api to access data.

This facade provides an interface/mechanism to implement customized mapping classes that can
be used to optimize performance for an object used a significant number of times. For large
result sets it can provide better performance than Dapper because the mapping class does a
direct assignment between the data row column and object property.

Of course on needs to remember that creating a direct mapping class requires more work than
using an alternative approach.



Execute a query and map the results to a parent/child class
------------------------------------------------------------
Example usage:
```csharp
    public class Ssn
    {
        public Guid SsnId { get; set; }
        public short Ssn1 { get; set; }
        public short Ssn2 { get; set; }
        public short Ssn3 { get; set; }
    }

    public class Users
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private Ssn _fk_Ssn;
        public Ssn fk_Ssn
        {
            get { return _fk_Ssn = _fk_Ssn ?? new Ssn(); }
            set { _fk_Ssn = value; }
        }

        public Guid SsnId
        {
            get { return fk_Ssn.SsnId; }
            set { fk_Ssn.SsnId = value; }
        }

        public short fk_Ssn_Ssn1
        {
            get { return fk_Ssn.Ssn1; }
            set { fk_Ssn.Ssn1 = value; }
        }
        public short fk_Ssn_Ssn2
        {
            get { return fk_Ssn.Ssn2; }
            set { fk_Ssn.Ssn2 = value; }
        }
        public short fk_Ssn_Ssn3
        {
            get { return fk_Ssn.Ssn3; }
            set { fk_Ssn.Ssn3 = value; }
        }
    }

    public class UsersMap : Dahl.Data.Common.Mapper<Users>
    {
        private readonly SsnMap _ssnMap = new SsnMap();

        #region Ordinal Variables -----------------------------------------------------------------
        private int _ordUserId;
        private int _ordFirstName;
        private int _ordLastName;
        private int _ordSsnId;
        #endregion

        public override void SetFieldOrdinals( ConcurrentDictionary<string, IFieldInfo> columns )
        {
            _ordUserId     = columns["UserId"].Ordinal;
            _ordFirstName  = columns["FirstName"].Ordinal;
            _ordLastName   = columns["LastName"].Ordinal;
            _ordSsnId      = columns["SsnId"].Ordinal;

            _ssnMap.SetFieldOrdinals( columns );
        }

        public override Users Map( object[] values )
        {
            Users users = new Users();
            users.SsnId     = values[_ordSsnId].ToString().ToGuid();
            users.UserId    = (int)values[_ordUserId];
            users.FirstName = (string)values[_ordFirstName];
            users.LastName  = (string)values[_ordLastName];
            users.fk_Ssn    = _ssnMap.Map( values );

            return users;
        }
    }

    public class SsnMap : Dahl.Data.Common.Mapper<Ssn>
    {
        #region Ordinal Variables -----------------------------------------------------------------
        private int _ordSsnId;
        private int _ordSsn1;
        private int _ordSsn2;
        private int _ordSsn3;
        #endregion

        /// <summary>
        /// called by the base class
        /// </summary>
        public override void SetFieldOrdinals( ConcurrentDictionary<string, Common.IFieldInfo> cols )
        {
            _ordSsnId = cols["SsnId"].Ordinal;
            _ordSsn1 = cols["Ssn1"].Ordinal;
            _ordSsn2 = cols["Ssn2"].Ordinal;
            _ordSsn3 = cols["Ssn3"].Ordinal;
        }

        public override Ssn Map( object[] values )
        {
            Ssn ssn = new Ssn();

            ssn.SsnId = (Guid)values[_ordSsnId];
            ssn.Ssn1 = (short)values[_ordSsn1];
            ssn.Ssn2 = (byte)values[_ordSsn2];
            ssn.Ssn3 = (short)values[_ordSsn3];

            return ssn;
        }
    }

```

