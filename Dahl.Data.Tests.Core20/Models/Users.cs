using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dahl.Data.Tests.Core20.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private Ssn _fk_Ssn;
        public Ssn fk_Ssn
        {
            get { return _fk_Ssn = _fk_Ssn ?? new Ssn(); }
            set { _fk_Ssn = value; }
        }

        public Guid SsnId
        {
            get { return fk_Ssn.SsnId; }
            set { fk_Ssn.SsnId = value; }
        }

        public short fk_Ssn_Ssn1
        {
            get { return fk_Ssn.Ssn1; }
            set { fk_Ssn.Ssn1 = value; }
        }
        public short fk_Ssn_Ssn2
        {
            get { return fk_Ssn.Ssn2; }
            set { fk_Ssn.Ssn2 = value; }
        }
        public short fk_Ssn_Ssn3
        {
            get { return fk_Ssn.Ssn3; }
            set { fk_Ssn.Ssn3 = value; }
        }
    }
}
