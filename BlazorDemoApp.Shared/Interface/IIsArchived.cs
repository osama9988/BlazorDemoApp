using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Interface
{
   public interface IIsArchived
    {
        bool IsArchived { get; set; }

    }

    public interface IIsArchivedInt: IIsArchived
    {
        DateTime? Archived_date { get; set; }

        int? Archived_by { get; set; }

    }
}
