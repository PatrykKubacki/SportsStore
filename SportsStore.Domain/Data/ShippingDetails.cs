using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Data
{

	public class ShippingDetails
	{
		[Required(ErrorMessage = "Proszę podać nazwisko")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Proszę podać pierwwszy wiersz adresu")]
		[Display(Name = "Wiersz 1")]
		public string Line1 { get; set; }
		[Display(Name = "Wiersz 2")]
		public string Line2 { get; set; }
		[Display(Name = "Wiersz 3")]
		public string Line3 { get; set; }

		[Required(ErrorMessage = "Proszę podać nazwe miasta")]
		[Display(Name = "Miasto")]
		public string City { get; set; }

		[Required(ErrorMessage = "Proszę podać nazwe województwa")]
		[Display(Name = "Województwo")]
		public string State { get; set; }

		public string Zip { get; set; }

		[Required(ErrorMessage = "Proszę podać nazwe państwa")]
		[Display(Name = "Państwo")]
		public string Country { get; set; }

		public bool GiftWrap { get; set; }
	}

}
