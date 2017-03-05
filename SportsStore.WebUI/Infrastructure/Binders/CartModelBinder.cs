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
			if (controllerContext.HttpContext.Session != null)
				cart = (Cart)controllerContext.HttpContext.Session[_sessionKey];

			if (cart != null) return cart;

			cart = new Cart();
			if (controllerContext.HttpContext.Session != null)
				controllerContext.HttpContext.Session[_sessionKey] = cart;

			return cart;
		}

	}

}
