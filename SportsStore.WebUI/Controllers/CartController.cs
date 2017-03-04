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

	    public ViewResult Index(string returnUrl)
	    {
		    return View(new CartIndexViewModel
		    {
			    Cart = GetCart(),
			    ReturnUrl = returnUrl
		    });
	    }

	    public RedirectToRouteResult AddtoCart(int Id, string returnUrl)
	    {
		    var product = _repository.Products.FirstOrDefault(p => p.Id == Id);

			if(product != null)
				GetCart().AddItem(product,1);

		    return RedirectToAction("Index", new { returnUrl });
	    }

		public RedirectToRouteResult RemoveFromCart(int Id, string returnUrl)
	    {
		    var product = _repository.Products.FirstOrDefault(p => p.Id == Id);

			if(product != null)
				GetCart().RemoveLine(product);

		    return RedirectToAction("Index", new { returnUrl });
	    }

	    Cart GetCart()
	    {
		    var cart = (Cart)Session["Cart"];
		    if (cart == null)
		    {
			    cart = new Cart();
			    Session["Cart"] = cart;
		    }

		    return cart;
	    }
    }
}