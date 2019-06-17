using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;

namespace Dahl.Data.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        public IServiceCollection Services      { get { return _services        ?? ( _services        = new ServiceCollection() ); } }
        public IServiceProvider ServiceProvider { get { return _serviceProvider ?? ( _serviceProvider = Services.BuildServiceProvider() ); } }
        protected TestRepository Repository     { get { return _repository      ?? ( _repository      = ServiceProvider.GetService<TestRepository>() ); } }

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
        public void SqlServer_LoadUsers()
        {
            Trace.Write( "SqlServer_LoadUsers: " );
            var result = Repository.LoadUsers() != null;
            TraceResult( result, "SqlServer_Open: " );
        }

        //[TestMethod]
        public void SqlServer_InsertUsers()
        {
            int count = 9;
            var result = Repository.InsertUsers( count );
            TraceResult( result == count * 2, "SqlServer_InsertNewUsers()" );
        }

        [TestMethod]
        public void SqlServer_ExecuteQueryLoadUsers()
        {
            Trace.Write( "SqlServer_ExecuteQueryLoadUsers: " );
            var result = Repository.LoadUsers();
            TraceResult( result != null, "SqlServer_ExecuteQueryLoadUsers()" );
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
