using System;
using System.Data.SqlClient;

namespace Dahl.Data.SqlServer
{
    public static class Extensions
    {
        public static T GetValueOrDefault<T>( this SqlParameter parameter )
        {
            if ( parameter.Value == DBNull.Value || parameter.Value == null )
            {
                if ( typeof( T ).IsValueType )
                    return (T)Activator.CreateInstance( typeof( T ) );

                return default;
            }

            return (T)parameter.Value;
        }
    }
}
