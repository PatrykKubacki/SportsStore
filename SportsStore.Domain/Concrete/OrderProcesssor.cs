using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.Domain.Concrete
{

	public class OrderProcesssor : IOrderProcessor
	{
		IEmailSender _emailSender;
	    IOrderRepository _orderRepository;
	    IOrderListRepository _orderListRepository;

		public OrderProcesssor(IEmailSender sender, IOrderRepository orderRepository, IOrderListRepository orderListRepository)
		{
		    _emailSender = sender;
		    _orderRepository = orderRepository;
		    _orderListRepository = orderListRepository;
		}

		public void ProcessOrder(Cart cart, User user)
		{
				var body = new StringBuilder();
				body.AppendLine("Nowe zamówienie")
					.AppendLine("---------")
					.AppendLine("Produkty:");

				foreach (var line in cart.Lines)
				{
					var subTotal = line.Quantity * line.Product.Price;
					var product = $"Nazwa: {line.Product.Name}, Ilość:{line.Quantity} razem: {subTotal.ToString("C", CultureInfo.CreateSpecificCulture("pl-pl"))}";
					body.AppendLine(product);
				}

			    body.AppendLine($"Wartość całkowita: {cart.ComputeTotalValue().ToString("C", CultureInfo.CreateSpecificCulture("pl-pl"))}")
			        .AppendLine("---")
			        .AppendLine("Wysyłka dla:")
			        .AppendLine($"{user.FirstName} {user.LastName}")
			        .AppendLine("---")
			        .AppendLine("Na adres:")
			        .AppendLine($"{user.Address.Code} {user.Address.City.Name} ")
			        .AppendLine($"Ulica: {user.Address.Street} Numer: {user.Address.Number}")
			        .AppendLine("---");

		    var order = CreateOrder(user);
            _orderRepository.SaveOrder(order);
		    var orderId = _orderRepository.Orders.FirstOrDefault(o => o.Name.Equals(order.Name)).Id;
		    var ordersList = CreateOrdersList(orderId, cart);
            _orderListRepository.SaveOrderList(ordersList);

            _emailSender.SendMessage(user.Email,"Złożono zamówienie w SportsStore",body.ToString());
		}

	    Order CreateOrder(User user)
	    {
	        return new Order
	        {
	            StatusId = 1,
	            UserId = user.Id,
	            Date = DateTime.Now,
	            Name = $"FAV-{Guid.NewGuid().ToString().Substring(0, 25)}"
            };
        }

	    IEnumerable<OrderList> CreateOrdersList(int orderId, Cart cart)
	    {
            var orderLists = new List<OrderList>();
            foreach (var line in cart.Lines)
            {
                orderLists.Add(new OrderList
                {
                    OrderId = orderId,
                    ProductId = line.Product.Id,
                    Quantity = line.Quantity
                });
            }
            return orderLists;
	    }

    }
}
