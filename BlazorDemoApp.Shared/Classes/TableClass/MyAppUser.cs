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
    public class MyAppUser : Base1_MyAppUser, IdInt, ITable
    {
        public int Id { get  ; set; }

        

        public bool? isEmp { get; set; }
        public bool? isParent { get; set; }

        public bool? isStd { get; set; }
        public bool? isPasswordReset { get; set; }

       
        public int? IDFK_MyAppUser { get; set; }
        public virtual MyAppUser? Reg_by { get; set; }


        public virtual ICollection<MyAppUser>? LMyAppUser { get; set; }
        public virtual ICollection<MyAppUserPermission>? LMyAppUserPermission { get; set; }
        public virtual ICollection<Add0_Gov>? LAdd0_Gov { get; set; }
        public virtual ICollection<Add1_Markaz>? LAdd1_Markaz { get; set; }
        public virtual ICollection<Add2_City>? LAdd2_City { get; set; }


    }
}
