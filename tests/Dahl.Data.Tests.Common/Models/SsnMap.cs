﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dahl.Data.Common;

namespace Dahl.Data.Tests.Common.Models
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
            Ssn ssn = new Ssn
            {
                SsnId = (Guid)values[_ordSsnId],
                Ssn1 = (short)values[_ordSsn1],
                Ssn2 = (byte)values[_ordSsn2],
                Ssn3 = (short)values[_ordSsn3]
            };

            return ssn;
        }
    }
}
