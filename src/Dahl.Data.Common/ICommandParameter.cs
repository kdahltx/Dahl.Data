using System.Data;
using System.Collections.Generic;

namespace Dahl.Data.Common
{
    public interface ICommandParameter : IEnumerable<IDataParameter>
    {
        void AddParameters( IDbCommand command );
    }
}
