using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Domain.Data
{

	public class Cart
	{
		List<CartLine> _cartLines = new List<CartLine>();
		public IEnumerable<CartLine> Lines => _cartLines;

		public void AddItem(Product product, int quantity)
		{
			CartLine line = _cartLines.FirstOrDefault(p => p.Product.Id == product.Id);

			if (line == null)
				_cartLines.Add(new CartLine { Product = product, Quantity = quantity });
			else
				line.Quantity += quantity;
		}

		public void RemoveLine(Product product)
		{
			_cartLines.RemoveAll(p => p.Product.Id == product.Id);
		}

	    public void RemoveOne(Product product)
	    {
	        var qty = _cartLines.Find(p => p.Product.Id == product.Id).Quantity;

	        if (qty > 1)
	            _cartLines.Find(p => p.Product.Id == product.Id).Quantity = --qty;
	    }

        public decimal ComputeTotalValue()
		{
			return _cartLines.Sum(p => p.Product.Price * p.Quantity);
		}

		public void Clear()
		{
			_cartLines.Clear();
		}
	}

	public class CartLine
	{
		public Product Product { get; set; }
		public int Quantity { get; set; }
	}
}
