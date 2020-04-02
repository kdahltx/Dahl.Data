using System;
using System.Data.Common;

namespace Dahl.Data.Common
{
    public class ProviderFactory : DbProviderFactory
    {
        public static readonly ProviderFactory Instance = new ProviderFactory();

        public DbProviderFactory Create( string providerName )
        {
            try
            {
#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1
                    return null;
#else
                return DbProviderFactories.GetFactory( providerName );
                #endif
            }
            catch
            {
                //TODO: log error.
                //LastError.Code = e.HResult;
                //LastError.Message = e.Message;
            }

            return null;
        }
    }
}
