using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        public ViewResult Index()
        {
            return View(repository.Games);
        }

        public ViewResult Edit(int gameId)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.GameId == gameId);
            return View(game);
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
        public ActionResult Edit(Game game, IFormFile image = null)
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
                repository.SaveGame(game);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(game);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Game());
        }

        [HttpPost]
        public ActionResult Delete(int gameId)
        {
            Game deletedGame = repository.DeleteGame(gameId);
            if (deletedGame != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                    deletedGame.Name);
            }
            return RedirectToAction("Index");
        }
    }
}