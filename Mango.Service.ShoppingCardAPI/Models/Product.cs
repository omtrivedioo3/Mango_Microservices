using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCardAPI.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string UserId { get; set; } = string.Empty; // Assuming UserId is a string, adjust as necessary
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 1000)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
    }
}
