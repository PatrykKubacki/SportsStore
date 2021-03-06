﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{

	[TestClass]
	public class PaginationTests
	{
		[TestMethod]
		public void CanPaginate()
		{
			var mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new List<Product>
			{
				new Product { Id = 1, Name = "P1" },
				new Product { Id = 2, Name = "P2" },
				new Product { Id = 3, Name = "P3" },
				new Product { Id = 4, Name = "P4" },
				new Product { Id = 5, Name = "P5" },
			});
			var controller = new ProductController(mock.Object) { PageSize = 3 };

			var result = (ProductListViewModel)controller.List(null, 2).Model;

			var products = result.Elements.ToArray();
			Assert.IsTrue(products.Length == 2);
			Assert.IsTrue(products[0].Name == "P4");
			Assert.IsTrue(products[1].Name == "P5");
		}

		[TestMethod]
		public void CanGeneratePageLinks()
		{
			HtmlHelper helper = null;
			var pagingInfo = new PagingInfo { TotalItems = 28, CurrentPage = 2, ItemsPerPage = 10 };
			Func<int, string> pageUrlDelegate = i => $"Strona{i}";

			var result = helper.PageLinks(pagingInfo, pageUrlDelegate);

			Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a>"
							+ @"<a class=""btn btn-default btn-primary selected"" href=""Strona2"">2</a>"
							+ @"<a class=""btn btn-default"" href=""Strona3"">3</a>",
							result.ToString());
		}

		[TestMethod]
		public void CanSendPaginationViewModel()
		{
			var mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new List<Product>
			{
				new Product { Id = 1, Name = "P1" },
				new Product { Id = 2, Name = "P2" },
				new Product { Id = 3, Name = "P3" },
				new Product { Id = 4, Name = "P4" },
				new Product { Id = 5, Name = "P5" },
			});
			var controller = new ProductController(mock.Object) { PageSize = 3 };
			var result = (ProductListViewModel)controller.List(null, 2).Model;

			var pageinfo = result.PagingInfo;
			Assert.AreEqual(pageinfo.CurrentPage, 2);
			Assert.AreEqual(pageinfo.ItemsPerPage, 3);
			Assert.AreEqual(pageinfo.TotalItems, 5);
			Assert.AreEqual(pageinfo.TotalPages, 2);
		}
	}

}
