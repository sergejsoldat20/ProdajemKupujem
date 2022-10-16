namespace ProdajemKupujem.Models.DTO
{
    public class ProductNameDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";

        public ProductNameDTO() { }

        public ProductNameDTO(Product product)
        {
            Id = product.Id;
            Name = product.Name;
        }
    }
}
