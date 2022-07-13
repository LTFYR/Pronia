using Pronia.Models.Base;

namespace Pronia.Models
{
    public class Slider:BaseEntity
    {
        public string Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
    }
}
