using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    public class Sale
    {
        [Key]
        public int IdSale { get; set; }

        public string SaleNumber { get; set; }
        public string? ClientName { get; set; } 
        public DateTime RegistrationDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Propiedad de navegación para la relación con 'SaleDetail'
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}
