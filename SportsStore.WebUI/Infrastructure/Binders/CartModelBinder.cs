using System;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Infrastructure.Binders
{

	public class CartModelBinder : IModelBinder
	{
		const string _sessionKey = "Cart";

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			Cart cart = null;
		    var user = HttpContext.Current.User.Identity.Name;
			if (controllerContext.HttpContext.Session != null)
				cart = (Cart)controllerContext.HttpContext.Session[_sessionKey];

			if (cart != null &&  controllerContext.HttpContext.Session["User"].Equals(user)) return cart;

			cart = new Cart();
		    if (controllerContext.HttpContext.Session != null)
		    {
				controllerContext.HttpContext.Session[_sessionKey] = cart;
		        controllerContext.HttpContext.Session["User"] = user;
		    }
            return cart;
		}

	}

}
