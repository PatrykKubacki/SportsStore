using System.Collections.Generic;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.Domain.Concrete
{

	public class EFAddressRepository : IAddressRepository
	{
		Entities _context = new Entities();
		public IEnumerable<Address> Addresses => _context.Addresses;
	}


	public class EFCategoryRepository : ICategoryRepository
	{
		Entities _context = new Entities();
		public IEnumerable<Category> Categories => _context.Categories;
	}


	public class EFCityRepository : ICityRepository
	{
		Entities _context = new Entities();
		public IEnumerable<City> Cities => _context.Cities;
	}


	public class EFOrderRepository : IOrderRepository
	{
		Entities _context = new Entities();
		public IEnumerable<Order> Orders => _context.Orders;
	}


	public class EFRoleRepository : IRoleRepository
	{
		Entities _context = new Entities();
		public IEnumerable<Role> Roles => _context.Roles;
	}


	public class EFUserRepository : IUserRepository
	{
		Entities _context = new Entities();
		public IEnumerable<User> Users => _context.Users;
	}


	public class EFProductRepository : IProductRepository
	{
		Entities _context = new Entities();
		public IEnumerable<Product> Products => _context.Products;

	    public void SaveProduct(Product product)
	    {
	        if (product.Id == 0)
	        {
	            _context.Products.Add(product);
	        }
	        else
	        {
	            Product dbEntry = _context.Products.Find(product.Id);
	            if (dbEntry != null)
	            {
	                dbEntry.Name = product.Name;
	                dbEntry.Description = product.Description;
	                dbEntry.Price = product.Price;
	                dbEntry.CategoryId = product.CategoryId;
	            }
	        }
	        _context.SaveChanges();
	    }

	    public Product DeleteProduct(int Id)
	    {
            Product dbEntry = _context.Products.Find(Id);
	        if (dbEntry == null) return null;

	        _context.Products.Remove(dbEntry);
	        _context.SaveChanges();
	        return dbEntry;
	    }
	}

}
