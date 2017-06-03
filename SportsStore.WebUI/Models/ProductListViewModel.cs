using System.Collections.Generic;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Models
{
    public interface IListViewModel<T>
    {
         IEnumerable<T> Elements { get; set; }
         PagingInfo PagingInfo { get; set; }
    }

    public class ProductListViewModel : IListViewModel<Product>
	{
	    public IEnumerable<Product> Elements { get; set; }
	    public PagingInfo PagingInfo { get; set; }
	    public string CurrentCategory { get; set; }
	}

    public class ListViewModel<T> : IListViewModel<T>
    {
        public IEnumerable<T> Elements { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

}
