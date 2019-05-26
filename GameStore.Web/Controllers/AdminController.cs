using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IGameRepository repository;

        public AdminController(IGameRepository repo)
        {
            repository = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await repository.GetGamesAsync());
        }

        public async Task<IActionResult> Edit(int gameId)
        {
            return View(await repository.FindAsync(gameId));
        }

        // Перегруженная версия Edit() для сохранения изменений
        //[HttpPost]
        //public ActionResult Edit(Game game)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        repository.SaveGame(game);
        //        TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        // Что-то не так со значениями данных
        //        return View(game);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(Game game, IFormFile image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    game.ImageMimeType = image.ContentType;
                    game.ImageData = new byte[image.Length];
                    using (var imageStream = image.OpenReadStream())
                    {
                        imageStream.Read(game.ImageData, 0, (int)image.Length);
                    }
                }
                await repository.SaveGameAsync(game);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(game);
            }
        }

        public IActionResult Create()
        {
            return View("Edit", new Game());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int gameId)
        {
            Game deletedGame = await repository.DeleteGameAsync(gameId);
            if (deletedGame != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                    deletedGame.Name);
            }
            return RedirectToAction("Index");
        }
    }
}