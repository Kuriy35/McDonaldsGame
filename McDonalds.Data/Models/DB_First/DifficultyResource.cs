using System;
using System.Collections.Generic;

namespace WpfApp1.Models.DB_First;

public partial class DifficultyResource
{
    public int Difficulty { get; set; }

    public string ResourceName { get; set; } = null!;

    public int BaseQuantity { get; set; }

    public decimal BuyPrice { get; set; }

    public decimal SellPrice { get; set; }
}
