using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class StatusController : Controller
    {
        IStatusRepository _repository;
        public int PageSize = 4;

        public StatusController(IStatusRepository repository)
        {
            _repository = repository;
        }

        [Authorize(Roles = "Administrator")]
        public ViewResult Index(int page = 1)
        {
            var model = new ListViewModel<Status>
            {
                Elements = _repository.Statuses.OrderBy(p => p.Id)
                                      .Skip((page - 1) * PageSize)
                                      .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Statuses.Count()
                }
            };
            return View(model);
        }

        public ViewResult Create()
        {
            return View("Edit", new Status());
        }

        public ViewResult Edit(int Id)
        {
            var status = _repository.Statuses.FirstOrDefault(p => p.Id == Id);
            return View(status);
        }

        [HttpPost]
        public ActionResult Edit(Status status)
        {
            if (!ModelState.IsValid) return View(status);

            _repository.SaveStatus(status);
            TempData["message"] = $"Zapisano {status.Name}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var deletedStatus = _repository.DeleteStatus(Id);
            if (deletedStatus != null)
                TempData["message"] = $"Usunięto {deletedStatus.Name}";

            return RedirectToAction("Index");
        }
    }

}
