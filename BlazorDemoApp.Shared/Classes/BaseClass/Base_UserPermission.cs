using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.BaseClass
{
    public class Base_UserPermission
    {
        [Required]
        public int? userID { get; set; }

        [Required]
        public short? AppServicesId { get; set; }

        public bool? Can_Insert { get; set; } = false;
        public bool? Can_search { get; set; } = false;

        public bool? Can_Update { get; set; } = false;
        public bool? Can_Delete { get; set; } = false;

        public bool? Can_print { get; set; } = false;
    }
}
