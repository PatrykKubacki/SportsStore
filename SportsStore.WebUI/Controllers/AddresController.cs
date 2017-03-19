using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Controllers
{
    public class AddresController : Controller
    {
        IAddressRepository _repository;

        public AddresController(IAddressRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index()
        {
            return View(_repository.Addresses);
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