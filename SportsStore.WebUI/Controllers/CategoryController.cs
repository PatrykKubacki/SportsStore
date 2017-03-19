using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepository _repository;

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.Categories);
        }

        public ViewResult Create()
        {
            return View("Edit",new Category());
        }

        public ViewResult Edit(int Id)
        {
            Category category = _repository.Categories.FirstOrDefault(c => c.Id == Id);
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if(!ModelState.IsValid) return View(category);
            _repository.SaveCategory(category);
            TempData["message"] = $"Zapisano {category.Name}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Category deletedCategory = _repository.DeleteCategory(Id);
            if (deletedCategory != null)
                TempData["message"] = $"Usunięto {deletedCategory.Name}";

            return RedirectToAction("Index");
        }
    }
}