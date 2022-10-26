namespace ProdajemKupujem.Models
{
    public class Comment : BaseModel
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User  { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
