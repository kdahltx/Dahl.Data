using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Dahl.Data.Common
{
    public class CommandParameter : List<IDataParameter>
    {
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public virtual void AddParameters(IDbCommand command)
        {
            if (command == null)
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat($"parameter count: {Count}{Environment.NewLine}");
            foreach (var parm in this)
            {
                try
                {
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;

                    command.Parameters.Add(parm);
                    sb.AppendFormat($"   {parm.ParameterName} = {parm.Value}{Environment.NewLine}");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
