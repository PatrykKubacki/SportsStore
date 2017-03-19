using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
	[TestClass]
	public class AdminTests
	{
		[TestMethod]
		public void IndexContainsAllProducts()
		{
			var mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { Id = 1, Name = "P1" },
				new Product { Id = 2, Name = "P2" },
				new Product { Id = 3, Name = "P3" },
			});

			var target = new ProductController(mock.Object);
			var result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

			Assert.AreEqual(result.Length,3);
			Assert.AreEqual("P1",result[0].Name);
			Assert.AreEqual("P2",result[1].Name);
			Assert.AreEqual("P3",result[2].Name);
		}

		[TestMethod]
		public void CanEditProduct()
		{
			var mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { Id = 1, Name = "P1" },
				new Product { Id = 2, Name = "P2" },
				new Product { Id = 3, Name = "P3" },
			});
			var target = new ProductController(mock.Object);

			var p1 = target.Edit(1).ViewData.Model as Product;
			var p2 = target.Edit(2).ViewData.Model as Product;
			var p3 = target.Edit(3).ViewData.Model as Product;

			Assert.AreEqual(1,p1?.Id);
			Assert.AreEqual(2,p2?.Id);
			Assert.AreEqual(3,p3?.Id);
		}

		[TestMethod]
		public void CanNotEditNoExistentProduct()
		{
			var mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { Id = 1, Name = "P1" },
				new Product { Id = 2, Name = "P2" },
				new Product { Id = 3, Name = "P3" },
			});
			var target = new ProductController(mock.Object);

			var result = target.Edit(4).ViewData.Model as Product;

			Assert.IsNull(result);
		}

        [TestMethod]
		public void CanSaveValidChanges()
		{
			var mock = new Mock<IProductRepository>();
			var target = new ProductController(mock.Object);
		    var product = new Product { Name = "test" };

		    ActionResult result = target.Edit(product);

            mock.Verify(m=>m.SaveProduct(product));
			Assert.IsNotInstanceOfType(result, typeof(ViewResult));
		}
        [TestMethod]
		public void CanSaveInValidChanges()
		{
			var mock = new Mock<IProductRepository>();
			var target = new ProductController(mock.Object);
		    var product = new Product { Name = "test" };
            target.ModelState.AddModelError("error","error");

		    ActionResult result = target.Edit(product);

            mock.Verify(m=>m.SaveProduct(It.IsAny<Product>()),Times.Never());
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

        [TestMethod]
		public void CanDeleteValidProducts()
		{
			Product product = new Product{ Id = 2, Name = "Test"};
            var mock = new Mock<IProductRepository>();
		    mock.Setup(m => m.Products).Returns(new Product[]
		    {
		        new Product{Id=1,Name = "P1"},
                product,
		        new Product{Id=3,Name = "P3"} 
		    });
            var target = new ProductController(mock.Object);

		    target.Delete(product.Id);

            mock.Verify(m=>m.DeleteProduct(product.Id));
		}

	    [TestMethod]
	    public void CanRetriveImageData()
	    {
	        Product product = new Product
	            {Id = 2,Name = "Test", ImageData = new byte[]{},ImageMimeType = "image/png"};
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{Id=1,Name = "P1"},
                product,
                new Product{Id=3,Name = "P3"}
            }.AsQueryable());
            var target = new ProductController(mock.Object);
                ActionResult result = target.GetImage(2);

            Assert.IsNotNull(result);
	        Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(product.ImageMimeType,((FileResult)result).ContentType);
            }
	}

}
