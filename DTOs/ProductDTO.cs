using System;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.DTOs
{
    public class ProductDTO
    {
        public int IdProduct { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }  
        public decimal PurchasePrice { get; set; }
        public int Stock { get; set; }

        // Llaves Foraneas
        public int IdCategory { get; set; }
        public int IdBrand { get; set; }

        // Propiedades descriptivas para la UI
        public string CategoryDescription { get; set; }
        public string BrandName { get; set; }
    }
}
