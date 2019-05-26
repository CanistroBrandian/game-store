using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GameStore.Web.Controllers
{
    public class GameController : Controller
    {
        private IGameRepository repository;
        public int pageSize = 4;

        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        public IActionResult List(string category, int page = 1)
        {
            GamesListViewModel model = new GamesListViewModel
            {
                Games = repository.GetByCategory(category, page, pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                repository.GetCount() :
                repository.GetCountByCategory(category)
                },
                CurrentCategory = category
            };
            return View(model);
        }
        public IActionResult GetImage(int gameId)
        {
            Game game = repository.Find(gameId);
            if (game != null)
            {
                return File(game.ImageData, game.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}