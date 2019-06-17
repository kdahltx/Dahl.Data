using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Common
{
    public class BulkMapper : IBulkMapper
    {
        public string TmpTableName { get; set; }
        public string SqlCreateTmpTable { get; set; }
        public virtual List<string> MapList { get; set; }

        public string[] DstTableName { get; set; }
        public string[] SqlMerge { get; set; }
    }
}
