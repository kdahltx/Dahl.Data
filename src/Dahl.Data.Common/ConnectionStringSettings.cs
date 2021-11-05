#if NETCOREAPP3_1 || NET5_0
using System;
using System.Diagnostics;
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
            //var cs = Microsoft.Extensions
            //                  .Configuration
            //                  .ConfigurationExtensions
            //                  .GetConnectionString( this.Configuration, connectionStringName );

            var currentDir = Directory.GetCurrentDirectory();
            IConfigurationBuilder cb = new ConfigurationBuilder().SetBasePath( currentDir )
                                                                 .AddJsonFile( "appsettings.json" );
            try {
                Configuration = cb.Build();
            }
            catch ( Exception e )
            {
                Trace.WriteLine( $"{e}" );
                throw e;
            }

            if ( connectionStringName.IsNotNullOrEmpty() )
                LoadConnectionString( connectionStringName );
        }

        public void LoadConnectionString( string connectionStringName = null )
        {
            if ( connectionStringName.IsNotNullOrEmpty() )
                ConnectionStringName = connectionStringName;

            if ( ConnectionStringName.IsNullOrEmpty() )
                throw new NullReferenceException( "ConnectionStringName undefined" );

            ConnectionString = Configuration.GetConnectionString( ConnectionStringName );
        }
    }
}
#endif
