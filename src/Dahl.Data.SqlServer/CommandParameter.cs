using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace Dahl.Data.SqlServer
{
    public class CommandParameter : Dahl.Data.Common.CommandParameter
    {
        /// <summary>
        /// Adds all parameters to the IDbCommand.Parameters property.
        /// </summary>
        /// <param name="command"></param>
        public override void AddParameters( IDbCommand command )
        {
            SqlCommand sqlCommand = command as SqlCommand;
            if ( sqlCommand == null )
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( $"parameter count: {Count}{Environment.NewLine}" );
            foreach ( var parm in this )
            {
                try
                {
                    if ( parm.Value == null )
                        parm.Value = DBNull.Value;

                    sqlCommand.Parameters.Add( parm );
                    sb.Append( $"   {parm.ParameterName} = {parm.Value}{Environment.NewLine}" );
                }
                catch ( Exception e )
                {
                    Debug.WriteLine( $"Exception Thrown: {e.Message}" );
                    throw;
                }
            }
        }
    }
}


