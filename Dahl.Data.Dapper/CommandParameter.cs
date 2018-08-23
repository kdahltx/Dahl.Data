using System.Data;
using Dapper;

namespace Dahl.Data.Dapper
{
    public class CommandParameter : Dahl.Data.Common.CommandParameter, SqlMapper.IDynamicParameters
    {
        public virtual void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            base.AddParameters(command);
        }
    }
}
