using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> List(string category, int page = 1)
        {
            GamesListViewModel model = new GamesListViewModel
            {
                Games = repository.GetByCategory(category, page, pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
              await  repository.GetCountAsync() :
              await  repository.GetCountByCategoryAsync(category)
                },
                CurrentCategory = category
            };
            return View(model);
        }
        public async Task<IActionResult> GetImage(int gameId)
        {
            Game game = await repository.FindAsync(gameId);
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