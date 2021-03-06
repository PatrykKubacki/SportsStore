﻿using System;
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
    public class CityController : Controller
    {
        ICityRepository _repository;
        public int PageSize = 4;

        public CityController(ICityRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index(int page = 1)
        {
            var model = new ListViewModel<City>
            {
                Elements = _repository.Cities.OrderBy(p => p.Id)
                                      .Skip((page - 1) * PageSize)
                                      .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Cities.Count()
                }
            };
            return View(model);
        }

        public ViewResult Create()
        {
            return View("Edit", new City());
        }

        public ViewResult Edit(int Id)
        {
            City city = _repository.Cities.FirstOrDefault(p => p.Id == Id);
            return View(city);
        }

        [HttpPost]
        public ActionResult Edit(City city)
        {
            if (!ModelState.IsValid) return View(city);

            _repository.SaveCity(city);
            TempData["message"] = $"Zapisano {city.Name}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            City deletedCity = _repository.DeleteCity(Id);
            if (deletedCity != null)
                TempData["message"] = $"Usunięto {deletedCity.Name}";

            return RedirectToAction("Index");
        }
    }
}