using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels
{
    public class LoginVM
    {
        [Required,StringLength(maximumLength:30)]
        public string userName { get; set; }
        [Required,DataType(DataType.Password)]
        public string password { get; set; }
        public bool rememberMe { get; set; }
    }
}
