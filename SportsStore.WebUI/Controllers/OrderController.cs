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
    }
}