using McDonalds.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McDonalds.Models
{
    public class DifficultyResource
    {
        [Key, Column(Order = 0)]
        public GameDifficulty Difficulty { get; set; }

        [Key, Column(Order = 1)]
        public string ResourceName { get; set; } = null!;
        
        [Required]
        public int BaseQuantity { get; set; }

        [Required] 
        public decimal BuyPrice { get; set; }
        
        [Required] 
        public decimal SellPrice { get; set; }
    }
}