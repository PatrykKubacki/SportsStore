using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoryController : Controller
    {
        ICategoryRepository _repository;
        public int PageSize = 4;

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index(int page = 1)
        {
            var model = new ListViewModel<Category>
            {
                Elements = _repository.Categories.OrderBy(p => p.Id)
                                      .Skip((page - 1) * PageSize)
                                      .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Categories.Count()
                }
            };
            return View(model);
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