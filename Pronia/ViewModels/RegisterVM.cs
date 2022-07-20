using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class RegisterVM
    {
        [Required,StringLength(maximumLength:30)]
        public string firstName { get; set; }
        [Required,StringLength(maximumLength:20)]
        public string lastName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required,StringLength(maximumLength:20)]
        public string username { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password),Compare(nameof(Password))]
        public string confirmPassword { get; set; }
        [Required]
        public bool Terms { get; set; }
    }
}
