using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    // Clase 'Detalle Venta': Para tener informacion adicional de los productos vendidos por cada venta.
    public class SaleDetail
    {
        [Key]
        public int IdSaleDetail { get; set; }

        public int Quantity { get; set; }
        
        // Precio histórico en el momento de la venta
        public decimal UnitPriceAtSale { get; set; }    

        // Llaves Foraneas
        public int IdSale {  get; set; }
        public int IdProduct { get; set; }

        // Propiedades de navegación
        public Sale? Sale { get; set; }
        public Product? Product { get; set; }
    }
}
