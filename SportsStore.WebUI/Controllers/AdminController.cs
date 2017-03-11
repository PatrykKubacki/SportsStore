using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
	    IProductRepository _repository;

	    public AdminController(IProductRepository repository)
	    {
		    _repository = repository;
	    }

	    public ViewResult Index()
        {
            return View(_repository.Products);
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

	    public ViewResult Edit(int Id)
	    {
		    Product product = _repository.Products.FirstOrDefault(p => p.Id == Id);
		    return View(product);
	    }

        [HttpPost]
        public ActionResult Edit(Product product)
	    {
	        if (!ModelState.IsValid) return View(product);

	        _repository.SaveProduct(product);
	        TempData["message"] = $"Zapisano {product.Name}";
	        return RedirectToAction("Index");
	    }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Product deletedProduct = _repository.DeleteProduct(Id);
            if (deletedProduct != null)
                TempData["message"] = $"Usunięto {deletedProduct.Name}";

            return  RedirectToAction("Index");
        }
    }
}