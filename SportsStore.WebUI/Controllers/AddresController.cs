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
    [Authorize(Roles = "Administrator")]
    public class AddresController : Controller
    {
        IAddressRepository _repository;
        public int PageSize = 4;

        public AddresController(IAddressRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index(int page = 1)
        {
            var model = new ListViewModel<Address>
            {
                Elements = _repository.Addresses.OrderBy(p => p.Id)
                                      .Skip((page - 1) * PageSize)
                                      .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Addresses.Count()
                }
            };
            return View(model);
        }

        public ViewResult Create()
        {
            ViewBag.Cities = new SelectList(_repository.Cities, "Id", "Name");
            return View("Edit", new Address());
        }

        public ViewResult Edit(int Id)
        {
            Address address = _repository.Addresses.FirstOrDefault(p => p.Id == Id);
            ViewBag.Cities = new SelectList(_repository.Cities, "Id", "Name", address?.CityId);
            return View(address);
        }

        [HttpPost]
        public ActionResult Edit(Address address)
        {
            if (!ModelState.IsValid) return View(address);

            _repository.SaveAddress(address);
            TempData["message"] = $"Zapisano {address.Street} - {address.Code} - {address.Number}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Address deletedAddress = _repository.DeleteAddress(Id);
            if (deletedAddress != null)
                TempData["message"] = $"Usunięto {deletedAddress.Street} - {deletedAddress.Code} - {deletedAddress.Number}";

            return RedirectToAction("Index");
        }
    }
}