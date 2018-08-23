using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Core20.Models
{
    public class SsnMap : Dahl.Data.Common.Mapper<Ssn>
    {
        #region Ordinal Variables -----------------------------------------------------------------
        private int _ordSsnId;
        private int _ordSsn1;
        private int _ordSsn2;
        private int _ordSsn3;
        #endregion

        /// <summary>
        /// called by the base class
        /// </summary>
        public override void SetFieldOrdinals( ConcurrentDictionary<string, IFieldInfo> columns )
        {
            _ordSsnId = columns["SsnId"].Ordinal;
            _ordSsn1 = columns["Ssn1"].Ordinal;
            _ordSsn2 = columns["Ssn2"].Ordinal;
            _ordSsn3 = columns["Ssn3"].Ordinal;
        }

        public override Ssn Map( object[] values )
        {
            Ssn ssn = new Ssn();

            ssn.SsnId = (Guid)values[_ordSsnId];
            ssn.Ssn1 = (short)values[_ordSsn1];
            ssn.Ssn2 = (byte)values[_ordSsn2];
            ssn.Ssn3 = (short)values[_ordSsn3];

            return ssn;
        }
    }
}
