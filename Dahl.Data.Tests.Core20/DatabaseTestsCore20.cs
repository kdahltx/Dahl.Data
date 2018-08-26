using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Core20
{
    public class DatabaseTestsCore20
    {
        public class BaseTest
        {
            private RepositoryTestCore20 _repository;
            protected RepositoryTestCore20 Repository
            {
                get { return _repository ?? ( _repository = new RepositoryTestCore20() ); }
                set { _repository = value; }
            }
        }

        [TestClass]
        public class CommonMethods : BaseTest
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

                List<DataProvider> providers = Repository.ProviderList;
                Assert.IsNotNull( providers, "Provider List returned null" );

                Trace.WriteLine( "------------------------------------------------------------------------" );
                foreach ( DataProvider provider in providers )
                {
                    Trace.WriteLine( $"Name: {provider.Name}, InvariantName: {provider.InvariantName}" );
                }
                Trace.WriteLine( "------------------------------------------------------------------------" );
            }
        }

        [TestClass]
        public class SqlServerMethods : BaseTest
        {
            [TestMethod]
            public void SqlServer_Connect()
            {
                Assert.IsNotNull( Repository, "_repository is null" );
                Trace.WriteLine( "Database connection to SqlServer successful!" );
            }

            [TestMethod]
            public void SqlServer_Open()
            {
                Assert.IsNotNull( Repository, "_repository is null" );
                var list = Repository.LoadUsers();
                Assert.IsNotNull( list );
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
                var list = Repository.InsertUsers();
                Assert.IsNotNull( list );
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
            public void SqlServer_ExecuteQuery_LoadUser()
            {
                var list = Repository.LoadOneUser();
                Assert.IsNotNull( list );
            }

            [TestMethod]
            public void SqlServer_ExecuteQuery_LoadAllUsers()
            {
                var list = Repository.LoadUsers();
                Assert.IsNotNull( list );
            }

            [TestMethod]
            public void SqlServer_ExecuteQuery_LoadUsers2()
            {
                var list = Repository.LoadUsers2();
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
                var list = Repository.BulkInsertUsers();
                Assert.IsNotNull( list );
            }

            ///------------------------------------------------------------------------------------
            /// <summary>
            /// This test method is different than other test methods above because it uses
            /// the golf database where the above methods use the Users database.
            /// 
            /// Requires connection string for:
            ///     database: Golf
            ///     server  : Server-db01\sql16
            /// </summary>
            //[TestMethod]
            public void SqlServer_QueryCourse()
            {
                var list = Repository.GetCourse( "Balcones" );
                Assert.IsNotNull( list );
            }
        }
    }
}
