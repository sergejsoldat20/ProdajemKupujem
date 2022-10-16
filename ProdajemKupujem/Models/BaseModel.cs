using System.ComponentModel.DataAnnotations;

namespace ProdajemKupujem.Models
{
    public class BaseModel
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Timestamp]
        public byte[] RowVersion { get; set; } = new byte[8];
        public DateTime CreatedDate { get; } = DateTime.Now;
    }
}
