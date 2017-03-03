using System.Collections.Generic;
using SportsStore.Domain.Data;

namespace SportsStore.Domain.Abstract
{

	public interface ICategoryRepository
	{
		IEnumerable<Category> Categories { get; }
	}

	public interface ICityRepository
	{
		IEnumerable<City> Cities { get; }
	}

	public interface IOrderRepository
	{
		IEnumerable<Order> Orders { get; }
	}

	public interface IProductRepository
	{
		IEnumerable<Product> Products { get; }
	}

	public interface IRoleRepository
	{
		IEnumerable<Role> Roles { get; }
	}

	public interface IUserRepository
	{
		IEnumerable<User> Users { get; }
	}

	public interface IAddressRepository
	{
		IEnumerable<Address> Addresses { get; }
	}

}
