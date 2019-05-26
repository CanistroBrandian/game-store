using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
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

        private readonly List<CartLine> _lineCollection;
        private readonly HttpContext _httpContext;
        public CookieCartProvider(IHttpContextAccessor httpContextAccessor, IGameRepository gameRepository)
        {
            _httpContext = httpContextAccessor.HttpContext;
            var cartCookie = _httpContext.Request.Cookies[CartCookieName];
            _lineCollection = new List<CartLine>();
            if (!string.IsNullOrEmpty(cartCookie))
            {
                var cookieCartLines = JsonConvert.DeserializeObject<List<CookieCartLine>>(cartCookie);
                var ids = cookieCartLines.Select(f => f.GameId);
                var games = gameRepository.GetGamesByIds(ids);
                foreach (var game in games)
                {
                    _lineCollection.Add(new CartLine
                    {
                        Game = game,
                        Quantity = cookieCartLines.First(s => s.GameId == game.GameId).Quantity
                    });
                }
            }
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
    }
}
