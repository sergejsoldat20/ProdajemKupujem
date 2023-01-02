using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProdajemKupujem.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string photoURL { get; set; } = "";
        public List<Product> Products { get; set; } = new List<Product>(); 
        public List<Comment> Comments { get; set; } = new List<Comment>();

    }
}
