using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
	
	public class ProductController : Controller
	{
		IProductRepository _repository;
		public int PageSize = 4;

		public ProductController(IProductRepository repository)
		{
			_repository = repository;
		}

		public ViewResult List(string category,int page = 1)
		{
			var model = new ProductListViewModel
			{
				Products = _repository.Products.Where(p => p.Category.Name == category || category == null)
											   .OrderBy(p => p.Id)
										       .Skip((page - 1) * PageSize)
											   .Take(PageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = _repository.Products.Count()
				},

				CurrentCategory = category
			};
			return View(model);
		}
	}

}
