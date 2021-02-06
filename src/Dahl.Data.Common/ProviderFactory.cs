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
#if NETCOREAPP2_1 || NETCOREAPP3_1 || NET5_0
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
