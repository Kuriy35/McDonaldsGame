using System.ComponentModel.DataAnnotations;

namespace McDonalds.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required] 
        public decimal BuyPrice { get; set; }

        [Required] 
        public decimal SellPrice { get; set; }
    }
}