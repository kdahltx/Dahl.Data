#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    public class ConnectionStringSettings
    {
        public static IConfigurationRoot Configuration { get; set; }

        public string Name { get; set; }
        public string ConnectionStringName { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public ConnectionStringSettings( string connectionStringName = null )
        {
            if ( connectionStringName.IsNotNullOrEmpty() )
                LoadConnectionStringSettings( connectionStringName );
        }

        public void LoadConnectionStringSettings( string connectionStringName )
        {
            var directory = Directory.GetCurrentDirectory();

            ConnectionStringName = connectionStringName;
            var builder = new ConfigurationBuilder().SetBasePath( directory )
                                                    .AddJsonFile( "appsettings.json" );
            Configuration = builder.Build();
            ConnectionString = Configuration.GetConnectionString( ConnectionStringName );
        }
    }
}
#endif
