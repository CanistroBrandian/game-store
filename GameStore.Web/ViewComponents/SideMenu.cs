using GameStore.Domain.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Web.ViewComponents
{
    public class SideMenu : ViewComponent
    {
        private readonly IGameRepository _repository;
        public SideMenu(IGameRepository repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            return View("Default", _repository.GetCategories());
        }
    }
}
