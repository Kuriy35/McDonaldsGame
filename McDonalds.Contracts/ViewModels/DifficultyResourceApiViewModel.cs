using System.ComponentModel.DataAnnotations;
using McDonalds.Models.Core;
using Swashbuckle.AspNetCore.Annotations;

namespace McDonalds.ViewModels.Api
{
    [SwaggerSchema("Ресурс для певної складності")]
    public class DifficultyResourceApiViewModel
    {
        [Required(ErrorMessage = "Складність обов'язкова")]
        public GameDifficulty Difficulty { get; set; }

        [Required(ErrorMessage = "Назва обов'язкова"), StringLength(50)]
        public string ResourceName { get; set; } = null!;

        [Range(0, 200, ErrorMessage = "Кількість від 0 до 200")]
        public int BaseQuantity { get; set; }

        [Range(0.01, 35, ErrorMessage = "Ціна від 0.01 і до 35.00")]
        public decimal BuyPrice { get; set; }

        [Range(0.01, 33, ErrorMessage = "Ціна від 0.01 і до 33.00")]
        public decimal SellPrice { get; set; }
    }
}