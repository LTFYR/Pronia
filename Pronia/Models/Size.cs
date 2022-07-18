using Pronia.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Size:BaseEntity
    {
        [Required, StringLength(maximumLength: 20)]
        public string Name { get; set; }
    }
}
