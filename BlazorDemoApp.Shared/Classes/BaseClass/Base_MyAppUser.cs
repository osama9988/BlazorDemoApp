using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.BaseClass
{
    public class Base0_MyAppUser_Login
    {
        [Required]
        [Display(Name = "اسم المستخدم")]
        public string? UserName { get; set; }

        [Required]
        [Display(Name = "كلمة السر")]
        public string? UserPass { get; set; }

    }
    public class Base0_MyAppUser_Register: Base0_MyAppUser_Login, IIsActive
    {
        [Display(Name = "الحالة")]
        public bool IsActive { get; set; } = true;

        [Required]
        public short? Q1 { get; set; }
        [Required]
        public string? Ans1 { get; set; }
        [Required]
        public short? Q2 { get; set; }
        [Required]
        public string? Ans2 { get; set; }
    }

    public partial class Base1_MyAppUser : Base0_MyAppUser_Register, IAuditEntity_EmpID_INT
    {
        
        public DateTime? added_date { get; set; }
        public int? added_by { get; set; }
        public DateTime? Modify_date { get; set; }
        public int? Modify_by { get; set; }
    }

   
}
