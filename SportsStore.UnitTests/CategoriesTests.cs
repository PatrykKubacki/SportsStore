using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{

	[TestClass]
	public class CategoriesTests
	{
		[TestMethod]
		public void CanFilterProducts()
		{
			var mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { Id = 1, Name = "P1", Category = new Category { Id = 1, Name = "CAT1" } },
				new Product { Id = 2, Name = "P2", Category = new Category { Id = 2, Name = "CAT2" } },
				new Product { Id = 3, Name = "P3", Category = new Category { Id = 1, Name = "CAT1" } },
				new Product { Id = 4, Name = "P4", Category = new Category { Id = 2, Name = "CAT2" } },
				new Product { Id = 5, Name = "P5", Category = new Category { Id = 3, Name = "CAT3" } },
			});
			var controller = new ProductController(mock.Object) { PageSize = 3 };

			var result = ((ProductListViewModel)controller.List("CAT2").Model).Elements.ToArray();

			Assert.AreEqual(result.Length, 2);
			Assert.IsTrue(result[0].Name == "P2" && result[0].Category.Name == "CAT2");
			Assert.IsTrue(result[1].Name == "P4" && result[1].Category.Name == "CAT2");
		}

		[TestMethod]
		public void CatCreateCategories()
		{
			var mock = new Mock<ICategoryRepository>();
			mock.Setup(m => m.Categories).Returns(new Category[]
			{
				new Category { Id = 1, Name = "CAT1" },
				new Category { Id = 1, Name = "CAT1" },
				new Category { Id = 2, Name = "CAT2" },
				new Category { Id = 3, Name = "CAT3" }
			});
			var controller = new NavController(mock.Object);

			var result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

			Assert.AreEqual(result.Length, 3);
			Assert.AreEqual(result[0], "CAT1");
			Assert.AreEqual(result[1], "CAT2");
			Assert.AreEqual(result[2], "CAT3");
		}

		[TestMethod]
		public void IndicatesSelectedCategory()
		{
			var mock = new Mock<ICategoryRepository>();
			mock.Setup(m => m.Categories).Returns(new Category[]
			{
				new Category { Id = 1, Name = "CAT1" },
				new Category { Id = 2, Name = "CAT2" },
			});
			var controller = new NavController(mock.Object);
			var categoryToSelect = "CAT1";

			var result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

			Assert.AreEqual(categoryToSelect, result);
		}
	}

}
