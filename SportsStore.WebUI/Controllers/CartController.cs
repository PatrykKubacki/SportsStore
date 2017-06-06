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
        IUserRepository _userRepository;

        public CartController(IProductRepository repository, IOrderProcessor orderProcessor, IUserRepository userRepository)
        {
            _repository = repository;
            _orderProcessor = orderProcessor;
            _userRepository = userRepository;
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

        public ActionResult AddOneToCart(Cart cart, int Id, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == Id);
            if (product != null)
                cart.AddItem(product, 1);

            return RedirectToAction("Index", new { returnUrl });
        }

        public ActionResult RemoveOneFromCart(Cart cart, int Id, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == Id);
            if (product != null)
                cart.RemoveOne(product);

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

        public ViewResult Checkout(Cart cart)
        {
            if (!cart.Lines.Any())
                ModelState.AddModelError("", "Koszyk jest pusty");

            if (!ModelState.IsValid) return View();

            var user = _userRepository.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            _orderProcessor.ProcessOrder(cart, user);
            cart.Clear();
            return View("Completed");
        }
    }
}
