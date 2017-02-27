using System.Collections.Generic;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{

	public class EFProductRepository : IProductRepository
	{
		EFDbContext _context = new EFDbContext();

		public IEnumerable<Product> Products => _context.Products;
	}

}
