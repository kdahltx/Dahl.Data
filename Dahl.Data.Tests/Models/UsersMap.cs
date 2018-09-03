using System.Collections.Concurrent;
using Dahl.Extensions;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Models
{
    public class UsersMap : Dahl.Data.Common.Mapper<Users>
    {
        private readonly SsnMap _ssnMap = new SsnMap();

        #region Ordinal Variables -----------------------------------------------------------------
        private int _ordUserId;
        private int _ordFirstName;
        private int _ordLastName;
        private int _ordSsnId;
        #endregion

        public override void SetFieldOrdinals( ConcurrentDictionary<string, IFieldInfo> columns )
        {
            _ordUserId     = columns["UserId"].Ordinal;
            _ordFirstName  = columns["FirstName"].Ordinal;
            _ordLastName   = columns["LastName"].Ordinal;
            _ordSsnId      = columns["SsnId"].Ordinal;

            _ssnMap.SetFieldOrdinals( columns );
        }

        public override Users Map( object[] values )
        {
            Users users = new Users();
            users.SsnId     = values[_ordSsnId].ToString().ToGuid();
            users.UserId    = (int)values[_ordUserId];
            users.FirstName = (string)values[_ordFirstName];
            users.LastName  = (string)values[_ordLastName];
            users.fk_Ssn    = _ssnMap.Map( values );

            return users;
        }
    }
}
