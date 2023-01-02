namespace ProdajemKupujem.Models.DTO
{
    public class ReceiverDTO
    {
        public ReceiverDTO() { }
        public ReceiverDTO(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public ReceiverDTO(ApplicationUser user)
        {
            Id = user.Id.ToString();
            Name = user.UserName;
        }

        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
