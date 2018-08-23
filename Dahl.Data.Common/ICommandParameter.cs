using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Dahl.Data.Common
{
    interface ICommandParameter : IEnumerable<IDataParameter>
    {
        void AddParameters( IDbCommand command );
    }
}
