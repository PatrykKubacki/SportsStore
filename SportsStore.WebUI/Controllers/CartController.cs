using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{

    public class CartController : Controller
    {
        IProductRepository _repository;
        IOrderProcessor _orderProcessor;

        public CartController(IProductRepository repository, IOrderProcessor orderProcessor)
        {
            _repository = repository;
            _orderProcessor = orderProcessor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddtoCart(Cart cart, int Id, string returnUrl, int quantity)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == Id);

            if (product != null)
                cart.AddItem(product, quantity);

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int Id, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == Id);

            if (product != null)
                cart.RemoveLine(product);

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (!cart.Lines.Any())
                ModelState.AddModelError("", "Koszyk jest pusty");

            if (!ModelState.IsValid) return View(new ShippingDetails());

            _orderProcessor.ProcessOrder(cart, shippingDetails);
            cart.Clear();
            return View("Completed");
        }
    }

}
