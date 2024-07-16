using System;

namespace BlazorDemoApp.Shared.Interface
{
    public interface IIdentity
    {

        DateTime? Added_date { get; set; }

        int Added_by { get; set; }

        DateTime? Modify_date { get; set; }

        int? Modify_by { get; set; }

    }

}
