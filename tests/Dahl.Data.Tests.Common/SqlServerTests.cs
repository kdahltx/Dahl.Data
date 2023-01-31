using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;

namespace Dahl.Data.Tests.Common
{
    //TODO: not sure this is needed anymore...
    [TestClass]
    public class SqlServerTests
    {
        private static readonly int count = 100000;

        public IServiceCollection Services      { get { return _services        ??= new ServiceCollection(); } }
        public IServiceProvider ServiceProvider { get { return _serviceProvider ??= Services.BuildServiceProvider(); } }
        protected TestRepository Repository     { get { return _repository      ??= ServiceProvider.GetService<TestRepository>(); } }

        [TestInitialize]
        public void Initialize()
        {
            Trace.WriteLine( "Dahl.Data.Tests.DataTests.Initialize()" );
            //Services.AddScoped<ILookupRepository, LookupRepository>();
            Services.AddScoped<TestRepository, TestRepository>();
        }

        [TestMethod]
        public void SqlServer_Connect()
        {
            Trace.Write( "SqlServer_Connect: " );
            var result = Repository != null;
            Trace.WriteLine( result ? "PASSED" : "FAILED" );
            Assert.IsTrue( result );
        }

        [TestMethod]
        public void SqlServer_BulkInsertUsers()
        {
            Repository.DeleteUsers();
            var result = Repository.BulkInsertUsers( count );
            TraceResult( result, "SqlServer_BulkInsertUsers()" );
        }

        [TestMethod]
        public void SqlServer_InsertUsers()
        {
            var result = Repository.InsertUsers( count );
            TraceResult( result == count * 2, "SqlServer_InsertNewUsers()" );
        }

        [TestMethod]
        public void SqlServer_LoadUsersUsingInnerJoin()
        {
            Trace.Write( "SqlServer_LoadUsersUsingInnerJoin: " );
            var result = Repository.LoadUsersUsingInnerJoin() != null;
            TraceResult( result, "SqlServer_Open: " );
        }

        [TestMethod]
        public void SqlServer_DeleteUsers()
        {
            var result = Repository.DeleteUsers();
            TraceResult( result >= 0, "SqlServer_DeleteUsers()" );
        }

        ///---------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="text"></param>
        private static void TraceResult( bool result, string text )
        {
            Trace.Write( result ? "PASSED" : "FAILED" );
            Trace.WriteLine( $": {text}" );
            if ( !result )
                Assert.Fail();
        }

        #region FIELDS ----------------------------------------------------------------------------
        private IServiceCollection _services;
        private IServiceProvider   _serviceProvider;
        private TestRepository     _repository;
        #endregion
    }
}
