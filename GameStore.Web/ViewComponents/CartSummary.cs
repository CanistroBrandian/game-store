using GameStore.Web.Models;
using GameStore.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.ViewComponents
{
    public class CartSummary : ViewComponent
    {
        private readonly ICartProvider _cartProvider;
        public CartSummary(ICartProvider cartProvider)
        {
            _cartProvider = cartProvider;
        }
        public IViewComponentResult Invoke()
        {
            var model = new CartSummaryViewModel
            {
                Lines = _cartProvider.Lines.ToList(),
                TotalValue = _cartProvider.ComputeTotalValue()
            };
            return View("Default", model);
        }
    }
}
