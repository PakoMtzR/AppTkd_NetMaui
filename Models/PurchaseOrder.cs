using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    // Clase 'Orden de Compra': Esta clase tiene como objetivo tener registro de las compras realizadas a mi proveedor.
    public class PurchaseOrder
    {
        [Key]
        public int IdPurchaseOrder { get; set; }

        public string PurchaseOrderNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal OrderCost { get; set; }
        public decimal ShipmentCost { get; set; }

        // Propiedad de navegación para la relación con 'PurchaseOrderDetail'
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
    }
}
