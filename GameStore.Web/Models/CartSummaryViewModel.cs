using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.Models
{
    public class CartSummaryViewModel
    {
        public List<CartLine> Lines { get; set; }

        public decimal TotalValue { get; set; }
    }
}
