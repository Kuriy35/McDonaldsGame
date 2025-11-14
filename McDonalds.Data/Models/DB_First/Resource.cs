using System;
using System.Collections.Generic;

namespace WpfApp1.Models.DB_First;

public partial class Resource
{
    public string Name { get; set; } = null!;

    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal BuyPrice { get; set; }

    public decimal SellPrice { get; set; }
}
