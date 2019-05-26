using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models
{
    public class CartIndexViewModel
    {
        public List<CartLine> Lines { get; set; }
        public string ReturnUrl { get; set; }   

        public decimal TotalValue { get; set; }
    }
}