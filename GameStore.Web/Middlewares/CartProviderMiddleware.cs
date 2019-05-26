using GameStore.Domain.Abstract;
using GameStore.Web.Models;
using GameStore.Web.Services.Concrete;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.Middlewares
{
    public class CartProviderMiddleware
    {

        private readonly RequestDelegate _next;

        public CartProviderMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, IGameRepository gameRepository)
        {
            var items = CookieCartProvider.GetCartItems(context);
            if (items != null)
            {
                throw new InvalidOperationException("No items should be in context on this stage!");
            }
            var cookieCartLines = CookieCartProvider.GetCartLinesFromCookie(context);
            var ids = cookieCartLines.Select(f => f.GameId);
            var games = await gameRepository.GetGamesByIdsAsync(ids);
            var lineCollection = new List<CartLine>();
            foreach (var game in games)
            {
                lineCollection.Add(new CartLine
                {
                    Game = game,
                    Quantity = cookieCartLines.First(s => s.GameId == game.GameId).Quantity
                });
            }
            CookieCartProvider.SetCartItems(context, lineCollection);
            await _next.Invoke(context);
        }
    }
}
