using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Common
{
    public class DataCollection<T> where T : class, new()
    {
        public int Length { get; set; }
        public List<T> Data { get; set; }
    }
}
