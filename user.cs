using System.ComponentModel.DataAnnotations;

namespace SimpleECommerce
{
    public class User
    {
        [Key] // Data Annotation
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Связь с заказами (навигационное свойство)
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
