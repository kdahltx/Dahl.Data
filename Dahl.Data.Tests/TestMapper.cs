using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dahl.Data.Common;

namespace Dahl.Data.Tests
{
    public class TestMapper<TEntity> : Common.Mapper<TEntity> where TEntity : class, new()
    {
        public TestMapper()
        {
        }

        public override TEntity Map( object[] values )
        {
            return null;
        }
    }
}
