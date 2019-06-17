using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dahl.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dahl.Data.Tests.Common
{
    public class DatabaseTests
    {
        public    IServiceCollection Services        { get { return _services        ?? ( _services = new ServiceCollection() ); } }
        public    IServiceProvider   ServiceProvider { get { return _serviceProvider ?? ( _serviceProvider = Services.BuildServiceProvider() ); } }
        protected TestRepository     Repository      { get { return _repository      ?? ( _repository = ServiceProvider.GetService<TestRepository>() ); } }

        public DatabaseTests()
        {
            DatabaseTestsInitialize();
        }

        public void DatabaseTestsInitialize()
        {
            Trace.WriteLine( "Dahl.Data.Tests.Common.DatabaseTests.Initialize()" );
            //Services.AddScoped<ILookupRepository, LookupRepository>();
            Services.AddScoped<TestRepository, TestRepository>();
        }

        public bool DatabaseProviders()
        {
            List<DataProvider> providers = Repository?.ProviderList;

            if ( providers == null )
                return false;

            Trace.WriteLine( "------------------------------------------------------------------------" );
            foreach ( DataProvider provider in providers )
            {
                Trace.WriteLine( $"Name: {provider.Name}, InvariantName: {provider.InvariantName}" );
            }

            Trace.WriteLine( "------------------------------------------------------------------------" );

            return true;
        }

        public bool SqlServer_Connect()
        {
            TraceResult( Repository != null, "SqlServer_Connect()" );

            return Repository != null;
        }

        public bool SqlServer_Open()
        {
            var result = Repository.LoadUsers() != null;
            TraceResult( result, "SqlServer_Open" );

            return result;
        }

        public bool SqlServer_InsertNewUsers()
        {
            int count = 9;
            var result = Repository.InsertUsers( count );
            TraceResult( result != count, "SqlServer_eInsertNewUsers()" );

            return result == count * 2;
        }

        public List<Models.Users> SqlServer_ExecuteQueryLoadUsers()
        {
            var result = Repository.LoadUsers();
            TraceResult( result != null, "SqlServer_ExecuteQueryLoadUsers()" );

            return result;
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
        }

        #region FIELDS ----------------------------------------------------------------------------
        private IServiceCollection _services;
        private IServiceProvider   _serviceProvider;
        private TestRepository     _repository;
        #endregion
    }
}
