using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProdajemKupujem.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Price { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; }
    }
}
