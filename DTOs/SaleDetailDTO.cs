namespace MauiApp1.DTOs
{
    public class SaleDetailDTO
    {
        public int IdSaleDetail { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        // Llaves Foráneas
        public int IdSale { get; set; }
        public int IdProduct { get; set; }

        // Propiedades informativas para la UI
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
