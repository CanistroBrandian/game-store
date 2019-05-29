using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Web.Models;
using GameStore.Web.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.Controllers
{
    namespace GameStore.Web.Controllers
    {
        public class CartController : Controller
        {
            private readonly IGameRepository _repository;
            private readonly IOrderProcessor _orderProcessor;
            private readonly ICartProvider _cartProvider;

            public CartController(IGameRepository repo, IOrderProcessor processor, ICartProvider cartProvider)
            {
                _repository = repo;
                _orderProcessor = processor;
                _cartProvider = cartProvider;
            }

            public IActionResult Checkout()
            {
                return View(new ShippingDetails());
            }

            [HttpPost]
            public IActionResult Checkout(ShippingDetails shippingDetails)
            {
                if (_cartProvider.Lines.Count() == 0)
                {
                    ModelState.AddModelError("", "Извините, ваша корзина пуста!");
                }

                if (ModelState.IsValid)
                {
                    _orderProcessor.ProcessOrder(shippingDetails);
                    _cartProvider.Clear();
                    return View("Completed");
                }
                else
                {
                    return View(shippingDetails);
                }
            }

            public IActionResult Index(string returnUrl)
            {
                return View(new CartIndexViewModel
                {
                    Lines = _cartProvider.Lines.ToList(),
                    ReturnUrl = returnUrl,
                    TotalValue = _cartProvider.ComputeTotalValue()
                });
            }

            public async Task<IActionResult> AddToCart(int gameId, string returnUrl)
            {
                Game game = await _repository.FindAsync(gameId);

                if (game != null)
                {
                     _cartProvider.AddItem(game, 1);
                }
                return RedirectToAction("Index", new { returnUrl });
            }

            public async Task<IActionResult> RemoveFromCart(int gameId, string returnUrl)
            {
                Game game = await _repository.FindAsync(gameId);

                if (game != null)
                {
                    _cartProvider.RemoveLine(game);
                }
                return RedirectToAction("Index", new { returnUrl });
            }
          
        }
    }
}