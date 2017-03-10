//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsStore.Domain.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Orders = new HashSet<Order>();
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Prosz� podaj nazwe produktu")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Range(0.01,double.MaxValue,ErrorMessage = "Prosz� poda� dodatni� cene")]
        public decimal Price { get; set; }

        [DataType(DataType.MultilineText), Display(Name="Opis")]
        [Required(ErrorMessage = "Prosz� podaj opis")]
        public string Description { get; set; }

        public Nullable<int> CategoryId { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
