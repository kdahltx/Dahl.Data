using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Common
{
    public interface IRepositoryBase
    {
        IDatabase Database { get; set; }
    }
}
