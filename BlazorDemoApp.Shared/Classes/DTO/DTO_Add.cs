﻿using BlazorDemoApp.Shared.Classes.BaseClass;
using BlazorDemoApp.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Classes.DTO
{
    public class DTO_Add
    {
        public class DTO_Gov : Base_Add0_Gov, Ikey
        {
            public string Key { get ; set ; }
        }

        public class DTO_Markaz : Base_Add1_Markaz, Ikey
        {
            public string Key { get ; set ; }
        }
    }
}