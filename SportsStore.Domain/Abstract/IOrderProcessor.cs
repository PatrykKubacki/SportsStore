using SportsStore.Domain.Data;

namespace SportsStore.Domain.Abstract
{

	public interface IOrderProcessor
	{
		void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
	}

}
