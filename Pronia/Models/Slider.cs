using Microsoft.AspNetCore.Http;
using Pronia.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }
        [Required(ErrorMessage = "Qiymeti daxil edin!")] 
        public string Price { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public int Order { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
