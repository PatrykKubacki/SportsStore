using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{

	public class NavController : Controller
	{
		ICategoryRepository _repository;

		public NavController(ICategoryRepository repository)
		{
			_repository = repository;
		}

		public PartialViewResult Menu(string category = null)
		{
			ViewBag.SelectedCategory = category;
			IEnumerable<string> categories = _repository.Categories.Select(c => c.Name)
																   .Distinct()
																   .OrderBy(c => c);
			return PartialView("FlexMenu",categories);
		}
	}

}
