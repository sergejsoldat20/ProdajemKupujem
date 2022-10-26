using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ProdajemKupujem.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Price { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
}
}
