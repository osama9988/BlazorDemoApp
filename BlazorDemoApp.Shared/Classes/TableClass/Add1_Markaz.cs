using BlazorDemoApp.Shared.Classes.BaseClass;
using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.TableClass
{
    public class Add1_Markaz : Base_Add1_Markaz, IdInt, ITable
    {
        public int Id { get; set; }

        public List<Add2_City> Add2_City { get; set; }
        public MyAppUser MyAppUser { get; set; }
        public Add0_Gov Add0_Gov { get; set; }
    }
   
}
