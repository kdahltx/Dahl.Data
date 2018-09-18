using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Net472
{
    [TestClass]
    public class DatabaseTests
    {
        [TestClass]
        public class Common_Methods
        {
            private Test _repository;
            protected Test Repository
            {
                get
                {
                    if ( _repository == null )
                        _repository = new Test();

                    return _repository;
                }
                set { _repository = value; }
            }

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
        public class SqlServer_Methods
        {
            private Test _repository;
            protected Test Repository
            {
                get
                {
                    if ( _repository == null )
                        _repository = new Test();

                    return _repository;
                }
                set { _repository = value; }
            }

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
            public void SqlServer_ExecuteQueryLoadUsers()
            {
                var list = Repository.LoadUsers();
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
        }
    }
}

