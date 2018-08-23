using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Tests.Core20.Models
{
    public class BulkMapperSsn : Dahl.Data.Common.BulkMapper
    {
        public BulkMapperSsn()
        {
            //TableName = "Ssn";
            //SqlMerge  = $"merge into {TableName} as dst " +
            //            $"using #tmp_Users as src " +
            //             "on dst.Id = src.Ssn_Id " +
            //             "when matched then " +
            //                 "update set " +
            //                     "dst.Ssn1 = src.Ssn_Ssn1, " +
            //                     "dst.Ssn2 = src.Ssn_Ssn2, " +
            //                     "dst.Ssn3 = src.Ssn_Ssn3 " +
            //             "when not matched then " +
            //                 "insert (Id,Ssn1,Ssn2,Ssn3) " +
            //                 "values (src.Ssn_Id,src.Ssn_Ssn1,src.Ssn_Ssn2,Ssn_Ssn3); ";
        }
    }
}

