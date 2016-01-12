using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warsztat.Models
{
    public class OrdersModel
    {
        public Orders Order { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}