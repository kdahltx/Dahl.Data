using System.Collections.Generic;

namespace Dahl.Data.Common
{
    public interface IBulkMapper
    {
        string TmpTableName { get; set; }
        string SqlCreateTmpTable { get; set; }
        List<string> MapList { get; set; }

        string[] DstTableName { get; set; }
        string[] SqlMerge { get; set; }
    }
}
