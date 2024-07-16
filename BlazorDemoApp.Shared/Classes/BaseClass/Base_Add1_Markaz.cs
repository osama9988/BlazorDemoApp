using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.BaseClass
{
    public class Base_Add1_Markaz : AuditEntity_EmpID_INT, INameAr, INameEn, IIsActive
    {
        [MinLength(3, ErrorMessage = "هذا الحقل لا يمكن ان يكون أقل من 3 حروف")]
        [MaxLength(150, ErrorMessage = "هذا الحقل لا يمكن ان يكون أكبر من 150 حرف")]
        public string NameAr { get; set; }

        [MinLength(3, ErrorMessage = "هذا الحقل لا يمكن ان يكون أقل من 3 حروف")]
        [MaxLength(150, ErrorMessage = "هذا الحقل لا يمكن ان يكون أكبر من 150 حرف")]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }

         public int IdFK_Gov { get; set; }

    }

}
