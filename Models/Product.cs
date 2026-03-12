using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    // Clase Producto: Informacion sobre los productos de la tienda.
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        public string Description { get; set; }
        public decimal SalePrice { get; set; }  
        public decimal PurchasePrice { get; set; }
        public int Stock { get; set; }
        
        // Llaves Foraneas  
        public int IdCategory { get; set; }
        public int IdBrand { get; set; }

        // Propiedades de navegación
        public Category? Category { get; set; }
        public Brand? Brand { get; set; }

        // Propiedad de navegación para la relación con 'SaleDetail'
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
        // Propiedad de navegación para la relación con 'PurchaseOrderDetail'
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
    }
}
