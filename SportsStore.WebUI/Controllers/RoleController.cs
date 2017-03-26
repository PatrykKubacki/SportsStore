using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        IRoleRepository _repository;

        public RoleController(IRoleRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index()
        {
            return View(_repository.Roles);
        }

        public ViewResult Create()
        {
            return View("Edit", new Role());
        }

        public ViewResult Edit(int Id)
        {
            Role product = _repository.Roles.FirstOrDefault(p => p.Id == Id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Role role, HttpPostedFileBase image = null)
        {
            if (!ModelState.IsValid) return View(role);

            _repository.SaveRole(role);
            TempData["message"] = $"Zapisano {role.Name}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Role deletedRole = _repository.DeleteRole(Id);
            if (deletedRole != null)
                TempData["message"] = $"Usunięto {deletedRole.Name}";

            return RedirectToAction("Index");
        }
    }
}