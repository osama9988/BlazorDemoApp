using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared.Interface
{
    public interface IId<T> where T : struct, IComparable<T>
    {
        
    }
    public interface IdShort : IId<short>
    {
         short Id { get; set; }
    }

    public interface IdInt : IId<int>
    {
        [Key]
        [Column(Order = 1, TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
    }

    public interface IdLong : IId<long>
    {
         long Id { get; set; }
    }

}
