using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace McDonalds.ViewModels.Api
{
    [SwaggerSchema("Ресурс для API")]
    public class ResourceApiViewModel
    {
        [Required(ErrorMessage = "Назва обов'язкова"), StringLength(50)]
        public string Name { get; set; } = null!;

        [Range(0, 200, ErrorMessage = "Кількість від 0 до 200")]
        public int Quantity { get; set; }

        [Range(0.01, 35, ErrorMessage = "Ціна від 0.01 і до 35.00")]
        public decimal BuyPrice { get; set; }

        [Range(0.01, 33, ErrorMessage = "Ціна від 0.01 і до 33.00")]
        public decimal SellPrice { get; set; }
    }
}