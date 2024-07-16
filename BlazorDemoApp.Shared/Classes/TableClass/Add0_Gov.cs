using BlazorDemoApp.Shared.Classes.BaseClass;
using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.TableClass
{
    public class Add0_Gov : Base_Add0_Gov, IdInt, ITable
    {
        public int Id { get ; set; }

        public List<Add1_Markaz> Markazs { get; set; }
       
        public MyAppUser MyAppUser { get; set; }    
    }
}
