using BlazorDemoApp.Shared.Classes.BaseClass;
using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.TableClass
{
    public class Add2_City : Base_Add2_City,IdInt, ITable
    {
        public int Id { get ; set; }
        public Add1_Markaz Add1_Markaz { get; set; }
        public MyAppUser MyAppUser { get; set; }
    }
}
