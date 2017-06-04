using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class ProductController : Controller
	{
		IProductRepository _repository;
		public int PageSize = 4;

        public ProductController(IProductRepository repository)
		{
			_repository = repository;

		}

        [AllowAnonymous]
        public ViewResult List(string category,int page = 1)
		{
			var model = new ProductListViewModel
			{
			    Elements = _repository.Products.Where(p => p.Category?.Name == category || category == null)
											   .OrderBy(p => p.Id)
										       .Skip((page - 1) * PageSize)
											   .Take(PageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = category == null ?
								_repository.Products.Count() :
								_repository.Products.Count(p => p.Category.Name == category)
				},

				CurrentCategory = category
			};
			return View(model);
		}

        [AllowAnonymous]
        public ViewResult FilteredList(string search = "",int page = 1)
		{
			var model = new ProductListViewModel
			{
			    Elements = _repository.Products.Where(p => p.Name.Contains(search))
											   .OrderBy(p => p.Id)
										       .Skip((page - 1) * PageSize)
											   .Take(PageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = _repository.Products.Count(p => p.Name.Contains(search)) 
				},

			};
			return View(model);
		}

        [AllowAnonymous]
        public FileContentResult GetImage(int Id)
	    {
	        Product product = _repository.Products.FirstOrDefault(p => p.Id == Id);
	        return product != null 
                ? File(product.ImageData, product.ImageMimeType) 
                : null;
	    }

        [Authorize(Roles = "Administrator")]
        public ViewResult Index(int page = 1)
        {
            var model = new ProductListViewModel
            {
                Elements = _repository.Products.OrderBy(p=>p.Id)
                                                .Skip((page-1) * PageSize)
                                                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Products.Count()
                }
            };
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ViewResult Create()
        {
            ViewBag.Categories = new SelectList(_repository.Categories, "Id", "Name");
            return View("Edit", new Product());
        }

        [Authorize(Roles = "Administrator")]
        public ViewResult Edit(int Id)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.Id == Id);
            ViewBag.Categories = product != null 
                        ? new SelectList(_repository.Categories, "Id", "Name", product.CategoryId) 
                        : new SelectList(_repository.Categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (!ModelState.IsValid) return View(product);

            if (image != null)
            {
                product.ImageMimeType = image.ContentType;
                product.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(product.ImageData, 0, image.ContentLength);
            }
            _repository.SaveProduct(product);
            TempData["message"] = $"Zapisano {product.Name}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int Id)
        {
            Product deletedProduct = _repository.DeleteProduct(Id);
            if (deletedProduct != null)
                TempData["message"] = $"Usunięto {deletedProduct.Name}";

            return RedirectToAction("Index");
        }
    }

}
