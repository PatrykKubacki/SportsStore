using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class OrderController : Controller
    {
        public int PageSize = 4;
        IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public ActionResult Index(int Id,int? page = 1)
        {
            var model = new ListViewModel<Order>
            {
                Elements = _orderRepository.Orders.OrderBy(o=>o.Date)
                                                  .Where(o=>o.UserId == Id)
                                                  .Skip((page.Value - 1) * PageSize)
                                                  .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page.Value,
                    ItemsPerPage = PageSize,
                    TotalItems = _orderRepository.Orders.Count()
                }
            };
            return View(model);
        }

        public ViewResult List(int page = 1)
        {
            var model = new ListViewModel<Order>
            {
                Elements = _orderRepository.Orders.OrderBy(p => p.Date)
                                      .Skip((page - 1) * PageSize)
                                      .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _orderRepository.Orders.Count()
                }
            };
            return View(model);
        }

        public ViewResult Edit(int Id)
        {
            var model = _orderRepository.Orders.FirstOrDefault(p => p.Id == Id);
            ViewBag.Statues = new SelectList(_orderRepository.Statues, "Id", "Name", model?.StatusId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (!ModelState.IsValid) return View(order);

            _orderRepository.SaveOrder(order);
            TempData["message"] = $"Zapisano {order.Name}";
            return RedirectToAction("List");
        }

    }
}