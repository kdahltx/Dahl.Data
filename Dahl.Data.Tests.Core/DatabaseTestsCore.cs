using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dahl.Data.Common;
using Dahl.Data.Tests.Common;

namespace Dahl.Data.Tests.Core
{
    public class DatabaseTestsCore
    {
        [TestClass]
        public class CommonMethods
        {
            [TestMethod]
            public void Common_DatabaseProviders()
            {

                //List<DataProvider> providers = Repository.ProviderList;
                //Assert.IsNotNull( providers, "Provider List returned null" );

                //Trace.WriteLine( "------------------------------------------------------------------------" );
                //foreach ( DataProvider provider in providers )
                //{
                //    Trace.WriteLine( $"Name: {provider.Name}, InvariantName: {provider.InvariantName}" );
                //}
                Trace.WriteLine( "------------------------------------------------------------------------" );
            }
        }

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


        //    ///------------------------------------------------------------------------------------
        //    /// <summary>
        //    /// This test method is different than other test methods above because it uses
        //    /// the golf database where the above methods use the Users database.
        //    /// 
        //    /// Requires connection string for:
        //    ///     database: Golf
        //    ///     server  : Server-db01\sql16
        //    /// </summary>
        //    //[TestMethod]
        //    public void SqlServer_QueryCourse()
        //    {
        //        var list = Repository.GetCourse( "Balcones" );
        //        Assert.IsNotNull( list );
        //    }
        //}
    }
}
