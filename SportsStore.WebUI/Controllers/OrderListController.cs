using System.Linq;
using System.Web.Mvc;
using Rotativa;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class OrderListController : Controller
    {
        IOrderListRepository _orderListRepository;

        public OrderListController(IOrderListRepository orderListRepository)
        {
            _orderListRepository = orderListRepository;
        }

        public ActionResult Index(int Id)
        {
            var model = _orderListRepository.OrdersList.Where(ol => ol.OrderId == Id);
            return View(model);
        }

        public ActionResult CreatePdf(int Id)
        {
            var model = _orderListRepository.OrdersList.Where(ol => ol.OrderId == Id);
            return View(model);
        }

        public ActionResult PrintInvoice(int Id)
        {
            return new ActionAsPdf("CreatePdf",new {Id = Id})
            {
                FileName = "Faktura.pdf"
            };
        }
    }
}