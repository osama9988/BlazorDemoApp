using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.BaseClass
{
    public class Base0_Add0_Gov :  INameAr, INameEn, IIsActive
    {
        [MinLength(3, ErrorMessage = "هذا الحقل لا يمكن ان يكون أقل من 3 حروف")]
        [MaxLength(150, ErrorMessage = "هذا الحقل لا يمكن ان يكون أكبر من 150 حرف")]
        public string NameAr { get; set; }

        [MinLength(3, ErrorMessage = "هذا الحقل لا يمكن ان يكون أقل من 3 حروف")]
        [MaxLength(150, ErrorMessage = "هذا الحقل لا يمكن ان يكون أكبر من 150 حرف")]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }


    }

    public class Base1_Add0_Gov : Base0_Add0_Gov, IAuditEntity_EmpID_INT
    {
        public DateTime? added_date { get; set; }
        public int? added_by { get; set; }
        public DateTime? Modify_date { get; set; }
        public int? Modify_by { get; set; }
    }
}

