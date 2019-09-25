using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Core22
{
    public class DatabaseTests
    {
        [TestCategory( "Core22.DatabaseTests" )]
        [TestClass]
        public class SqlServer_Methods
        {
            private Dahl.Data.Tests.Common.DatabaseTests _dbTests;
            public Dahl.Data.Tests.Common.DatabaseTests DbTests
            {
                get
                {
                    if ( _dbTests == null )
                        _dbTests = new Dahl.Data.Tests.Common.DatabaseTests();
                    return _dbTests;
                }

                set { _dbTests = value; }
            }

            [TestMethod]
            public void SqlServer_Connect()
            {
                Trace.Write( "SqlServer_Connect: " );
                var result = DbTests.SqlServer_Connect();
                Trace.WriteLine( result ? "PASSED" : "FAILED" );
                Assert.IsTrue( result );
            }

            [TestMethod]
            public void SqlServer_Open()
            {
                Trace.Write( "SqlServer_Open: " );
                var result = DbTests.SqlServer_Open();
                Trace.WriteLine( result ? "PASSED" : "FAILED" );
                Assert.IsTrue( result );
            }

            [TestMethod]
            public void SqlServer_InsertUsers()
            {
                var result = DbTests.SqlServer_InsertNewUsers();
                Trace.WriteLine( $"SqlServer_InsertUsers: {(result ? "PASSED" : "FAILED")}" );
                Assert.IsTrue( result );
            }

            [TestMethod]
            public void SqlServer_ExecuteQueryLoadUsers()
            {
                Trace.Write( "SqlServer_ExecuteQueryLoadUsers: " );
                var list = DbTests.SqlServer_ExecuteQueryLoadUsers();
                Trace.WriteLine( list != null ? "PASSED" : "FAILED" );
                Assert.IsNotNull( list );
            }
        }
    }
}

