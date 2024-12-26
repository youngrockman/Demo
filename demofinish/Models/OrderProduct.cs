using System;
using System.Collections.Generic;

namespace demofinish.Models;

public partial class OrderProduct
{
    public int Orderid { get; set; }

    public string Productarticlenumber { get; set; } = null!;

    public int Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductarticlenumberNavigation { get; set; } = null!;
}
