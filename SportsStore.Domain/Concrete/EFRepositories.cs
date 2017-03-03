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
	}

}
