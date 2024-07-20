using BlazorDemoApp.Shared.Classes.BaseClass;
using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.TableClass
{
    public class MyAppUserPermission : Base_UserPermission, ITable, IdInt, IAuditEntity_EmpID_INT
    {
        [Key]
        [Column(Order = 1, TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get ; set ; }

        public DateTime? added_date { get ; set ; }
        public int? added_by { get ; set ; }
        public DateTime? Modify_date { get ; set ; }
        public int? Modify_by { get ; set ; }

        public MyAppUser MyAppUser { get; set; }
    }
}
