using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    public class SaleDetail
    {
        [Key]
        public int IdSaleDetail { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        // Llaves Foraneas
        public int IdSale {  get; set; }
        public int IdProduct { get; set; }

        // Propiedades de navegación
        public Sale Sale { get; set; }
        public Product Product { get; set; }
    }
}
