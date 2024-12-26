using System;
using System.Collections.Generic;

namespace demofinish.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public string Orderstatus { get; set; } = null!;

    public DateTime? Orderdeliverydate { get; set; }

    public int Orderpickuppoint { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
