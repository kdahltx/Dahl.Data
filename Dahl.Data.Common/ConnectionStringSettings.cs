#if NETCOREAPP2_0 || NETCOREAPP2_1
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Dahl.Extensions;

namespace Dahl.Data.Common
{
    public class ConnectionStringSettings
    {
        public static IConfigurationRoot Configuration { get; set; }

        public ConnectionStringSettings()
        {
            IConfigurationBuilder cb = new ConfigurationBuilder();
            cb = cb.SetBasePath( Directory.GetCurrentDirectory() );
            cb = cb.AddJsonFile( "appsettings.json" );
            Configuration = cb.Build();
        }

        public void LoadConnectionString( string connectionStringName = null )
        {
            if ( connectionStringName.IsNotNullOrEmpty() )
                ConnectionStringName = connectionStringName;

            if ( ConnectionStringName.IsNullOrEmpty() )
                throw new NullReferenceException( "ConnectionStringName undefined" );

            ConnectionString = Configuration.GetConnectionString( ConnectionStringName );
        }

        public string Name { get; set; }
        public string ConnectionStringName { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
#endif