using System;
using System.Collections.Generic;
using System.Text;

namespace Dahl.Data.Tests.Core.Models
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public int FacilityId { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }

        //virtual public Facility fk_Facility { get; set; }
        //virtual public List<Address> fk_Addresses { get; set; }
        //virtual public List<TeeBox> fk_TeeBoxes { get; set; }
    }
}
