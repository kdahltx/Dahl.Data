using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Tests.Models
{
    public class UsersBulkMapper : Dahl.Data.Common.BulkMapper
    {

        public UsersBulkMapper()
        {
            DstTableName = new string[] { "Users","Ssn" };
            TmpTableName = "User_Ssn";

            SqlCreateTmpTable = $"create table #tmp_{TmpTableName} (" +
                                $"UserId int," +
                                $"FirstName varchar(32)," +
                                $"LastName varchar(32)," +
                                $"SsnId uniqueidentifier," +

                                $"fk_Ssn_Ssn1 smallint," +
                                $"fk_Ssn_Ssn2 tinyint," +
                                $"fk_Ssn_Ssn3 smallint )";

            SqlMerge = new string[] {
                $"merge into {DstTableName[0]} as dst " +
                $"using #tmp_{TmpTableName} as src " +
                "on dst.UserId = src.UserId " +
                "when matched then " +
                    "update set " +
                        "dst.FirstName = src.FirstName, " +
                        "dst.LastName = src.LastName, " +
                        "dst.SsnId = src.SsnId " +
                "when not matched then " +
                    "insert (FirstName,LastName,SsnId) " +
                    "values (src.FirstName,src.LastName,src.SsnId); ",

                // merges into Ssn table
                $"merge into {DstTableName[1]} as dst " +
                $"using #tmp_{TmpTableName} as src " +
                "on dst.SsnId = src.SsnId " +
                "when matched then " +
                    "update set " +
                        "dst.Ssn1 = src.fk_Ssn_Ssn1, " +
                        "dst.Ssn2 = src.fk_Ssn_Ssn2, " +
                        "dst.Ssn3 = src.fk_Ssn_Ssn3 " +
                "when not matched then " +
                    "insert (SsnId,Ssn1,Ssn2,Ssn3) " +
                    "values (src.SsnId,src.fk_Ssn_Ssn1,src.fk_Ssn_Ssn2,fk_Ssn_Ssn3); "
            };
        }

        public override List<string> MapList
        {
            get { return _mapList; }
            set { base.MapList = value; }
        }

        private List<string> _mapList = new List<string> {
                                            "UserId,UserId",
                                            "FirstName,FirstName",
                                            "LastName,LastName",
                                            "SsnId,SsnId",
                                            "fk_Ssn_Ssn1,fk_Ssn_Ssn1",
                                            "fk_Ssn_Ssn2,fk_Ssn_Ssn2",
                                            "fk_Ssn_Ssn3,fk_Ssn_Ssn3"
                                        };
    }
}
