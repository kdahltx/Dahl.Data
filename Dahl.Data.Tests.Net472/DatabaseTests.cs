using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Net472
{
    [TestCategory("NET472.DatabaseTests")]
    [TestClass]
    public class DatabaseTests
    {
        [TestClass]
        public class Common_Methods
        {
            [TestMethod]
            public void StringToInt()
            {
                for ( int i = 0; i < 1000; i++ )
                    Trace.WriteLine( $"i=[{i:D6}]" );
            }

            [TestMethod]
            public void Common_Connect()
            {
            }

            [TestMethod]
            public void Common_DatabaseProviders()
            {
            }
        }

        [TestClass]
        public class SqlServer_Methods
        {
            public Dahl.Data.Tests.Common.DatabaseTests DbTests { get; set; }

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

            //[TestMethod]
            //public void SqlServer_ExecuteCommandWithParametersNone()
            //{
            //    var list = Repository.InsertUsers();
            //    Assert.IsNotNull( list );
            //}

            [TestMethod]
            public void SqlServer_ExecuteCommandInsertUsers()
            {
                Trace.Write( "SqlServer_Open: " );
                var result = DbTests.SqlServer_Open();
                Trace.WriteLine( result ? "PASSED" : "FAILED" );
                Assert.IsTrue( result );

                //var list = Repository.InsertUsers();
                //Assert.IsNotNull( list );
            }

            //[TestMethod]
            //public void SqlServer_ExecuteNamedCommandWithParametersNone()
            //{
            //    var list = Repository.InsertUsers();
            //    Assert.IsNotNull( list );
            //}

            //[TestMethod]
            //public void SqlServer_ExecuteNamedCommandWithParameters()
            //{
            //    var list = Repository.InsertUsers();
            //    Assert.IsNotNull( list );
            //}

            [TestMethod]
            public void SqlServer_ExecuteQueryLoadUsers()
            {
                Trace.Write( "SqlServer_ExecuteQueryLoadUsers: " );
                var list = DbTests.SqlServer_ExecuteQueryLoadUsers();
                Trace.WriteLine( list != null ? "PASSED" : "FAILED" );
                Assert.IsNotNull( list );
            }

            //[TestMethod]
            //public void SqlServer_ExecuteQueryWithParameters()
            //{
            //    var list = Repository.LoadUsers();
            //    Assert.IsNotNull( list );
            //}

            //[TestMethod]
            //public void SqlServer_ExecuteNamedQueryWithParametersNone()
            //{
            //    var list = Repository.LoadUsers();
            //    Assert.IsNotNull( list );
            //}

            //[TestMethod]
            //public void SqlServer_ExecuteNamedQueryWithParameters()
            //{
            //    var list = Repository.LoadUsers();
            //    Assert.IsNotNull( list );
            //}

            [TestMethod]
            public void SqlServer_BulkInsertUsers()
            {
                //var list = Repository.BulkInsertUsers();
                //Assert.IsNotNull( list );
            }
        }
    }
}

