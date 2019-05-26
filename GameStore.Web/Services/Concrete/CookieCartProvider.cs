using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Web.Middlewares;
using GameStore.Web.Models;
using GameStore.Web.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.Web.Services.Concrete
{
    public class CookieCartProvider : ICartProvider
    {
        private const string CartCookieName = "Cart";
        private const string CartContextKey = "CartItems";

        private readonly List<CartLine> _lineCollection;
        private readonly HttpContext _httpContext;
        public CookieCartProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            var items = GetCartItems(_httpContext);
            _lineCollection = items ?? throw new InvalidOperationException("Items shoud be provided");
        }
        public void AddItem(Game game, int quantity)
        {
            CartLine line = _lineCollection
                .Where(g => g.Game.GameId == game.GameId)
                .FirstOrDefault();

            if (line == null)
            {
                _lineCollection.Add(new CartLine
                {
                    Game = game,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
            SaveChanges();
        }

        public void RemoveLine(Game game)
        {
            _lineCollection.RemoveAll(l => l.Game.GameId == game.GameId);
            SaveChanges();
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(e => e.Game.Price * e.Quantity);

        }
        public void Clear()
        {
            _lineCollection.Clear();
            SaveChanges();
        }

        public IEnumerable<CartLine> Lines => _lineCollection;

        private void SaveChanges()
        {
            var cookieList = _lineCollection.Select(s => new CookieCartLine
            {
                GameId = s.Game.GameId,
                Quantity = s.Quantity
            }).ToList();
            var jsonString = JsonConvert.SerializeObject(cookieList);
            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions()
            {
                //Path = "/",
                HttpOnly = false,
                IsEssential = true, //<- there
                Expires = DateTime.Now.AddMonths(1),
            };
            _httpContext.Response.Cookies.Append(CartCookieName, jsonString, cookieOptions);
        }

        public static void SetCartItems(HttpContext context, List<CartLine> items)
        {
            context.Items[CartContextKey] = items;
        }

        public static List<CartLine> GetCartItems(HttpContext context)
        {
            return context.Items[CartContextKey] as List<CartLine>;
        }

        public static List<CookieCartLine> GetCartLinesFromCookie(HttpContext context)
        {
            var cartCookie = context.Request.Cookies[CartCookieName];
            if (!string.IsNullOrEmpty(cartCookie))
            {
                return JsonConvert.DeserializeObject<List<CookieCartLine>>(cartCookie);
            }
            return new List<CookieCartLine>();
        }
    }
}
