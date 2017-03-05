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
    public class CartController : Controller
    {
	    IProductRepository _repository;

	    public CartController(IProductRepository repository)
	    {
		    _repository = repository;
	    }

	    public ViewResult Index(Cart cart,string returnUrl)
	    {
		    return View(new CartIndexViewModel
		    {
			    Cart = cart,
			    ReturnUrl = returnUrl
		    });
	    }

	    public RedirectToRouteResult AddtoCart(Cart cart,int Id, string returnUrl)
	    {
		    var product = _repository.Products.FirstOrDefault(p => p.Id == Id);

			if(product != null)
				cart.AddItem(product,1);

		    return RedirectToAction("Index", new { returnUrl });
	    }

		public RedirectToRouteResult RemoveFromCart(Cart cart,int Id, string returnUrl)
	    {
		    var product = _repository.Products.FirstOrDefault(p => p.Id == Id);

			if(product != null)
				cart.RemoveLine(product);

		    return RedirectToAction("Index", new { returnUrl });
	    }

	    public PartialViewResult Summary(Cart cart)
	    {
		    return PartialView(cart);
	    }
  
    }
}