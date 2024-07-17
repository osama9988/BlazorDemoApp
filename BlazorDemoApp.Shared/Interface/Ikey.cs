using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Interface
{
    public interface Ikey
    {
        string Key { get; set; }
    }

    public interface IkeyIdentity
    {
        string Key { get; set; }
        string? AddedBy { get; set; }

        string? ModifiedBy { get; set; }
    }
}
