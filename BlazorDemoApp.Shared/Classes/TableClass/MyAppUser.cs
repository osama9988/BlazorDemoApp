using BlazorDemoApp.Shared.Classes.BaseClass;
using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.TableClass
{
    public class MyAppUser : AuditEntity_EmpID_INT, IdInt,IIsActive, ITable
    {
        public int Id { get  ; set; }

        [Display(Name = "اسم المستخدم")]
        public string? UserName { get; set; }
        [Display(Name = "كلمة السر")]
        public string? UserPass { get; set; }
        public short? Q1 { get; set; }
        public string? Ans1 { get; set; }
        public short? Q2 { get; set; }
        public string? Ans2 { get; set; }

        public bool? isEmp { get; set; }
        public bool? isParent { get; set; }

        public bool? isStd { get; set; }
        public bool? isPasswordReset { get; set; }

        [Display(Name = "الحالة")]
        public bool IsActive { get; set; } = true;
        public int? IDFK_MyAppUser { get; set; }
        public virtual MyAppUser? Reg_by { get; set; }


        public virtual ICollection<MyAppUser>? LMyAppUser { get; set; }
        public virtual ICollection<Add0_Gov>? Add0_Gov { get; set; }
        public virtual ICollection<Add1_Markaz>? Add1_Markaz { get; set; }
        public virtual ICollection<Add2_City>? Add2_City { get; set; }


    }
}
