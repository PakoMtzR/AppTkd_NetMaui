using System;
using System.Collections.Generic;

namespace MauiApp1.DTOs
{
    public class SaleDTO
    {
        public int IdSale { get; set; }
        public string SaleNumber { get; set; }
        public string? ClientName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Relación con los detalles
        public List<SaleDetailDTO> SaleDetails { get; set; } = new List<SaleDetailDTO>();
    }
}
