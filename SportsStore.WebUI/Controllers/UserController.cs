using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Controllers
{
    public class UserController : Controller
    {
        IUserRepository _repository;
        public int PageSize = 4;

        public UserController(IUserRepository repository)
        {
            _repository = repository;

        }
        public ViewResult Index()
        {

            return View(_repository.Users);
        }
        public ViewResult Create()
        {
            ViewBag.Addresses = new SelectList(_repository.Addresses, "Id", "Street");
            ViewBag.Roles = new SelectList(_repository.Roles, "Id", "Name");
            return View("Edit", new User());
        }

        public ViewResult Edit(int Id)
        {
            User user = _repository.Users.FirstOrDefault(p => p.Id == Id);
            ViewBag.Addresses = new SelectList(_repository.Addresses, "Id", "Street", user?.AddressId);
            ViewBag.Roles = new SelectList(_repository.Roles, "Id", "Name", user?.RoleId);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (!ModelState.IsValid) return View(user);

            _repository.SaveUser(user);
            TempData["message"] = $"Zapisano {user.FirstName}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            User deletedUser = _repository.DeleteUser(Id);
            if (deletedUser != null)
                TempData["message"] = $"Usunięto {deletedUser.FirstName}";

            return RedirectToAction("Index");
        }
    }
}