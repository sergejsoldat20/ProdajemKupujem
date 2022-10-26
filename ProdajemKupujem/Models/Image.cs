namespace ProdajemKupujem.Models
{
    public class Image : BaseModel
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string ImageUrl { get; set; }
    }
}
