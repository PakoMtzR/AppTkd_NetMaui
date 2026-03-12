using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    public class PurchaseOrderDetail
    {
        [Key]
        public int IdPurchaseOrderDetail { get; set; }

        public int Quantity { get; set; }
        
        // Precio histórico pactado con el proveedor al momento de la orden
        public decimal UnitPriceAtPurchase { get; set; }

        // Llaves Foraneas
        public int IdPurchaseOrder { get; set; }
        public int IdProduct { get; set; }

        // Propiedades de navegación
        public PurchaseOrder? PurchaseOrder { get; set; }
        public Product? Product { get; set; }
    }
}
