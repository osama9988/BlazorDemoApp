using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.BaseClass
{

    public interface IAuditEntity_EmpID_INT
    {
        public DateTime? added_date { get; set; }

        public int? added_by { get; set; }


        public DateTime? Modify_date { get; set; }

        public int? Modify_by { get; set; }

    }
    public class AuditEntity_EmpID_INT: IAuditEntity_EmpID_INT
    {

        public DateTime? added_date { get; set; }

        public int? added_by { get; set; }


        public DateTime? Modify_date { get; set; }

        public int? Modify_by { get; set; }

    }
}
