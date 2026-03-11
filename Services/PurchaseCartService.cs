using MauiApp1.DTOs;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Services
{
    public class PurchaseCartService
    {
        // Lista de productos en el carrito de compras (pedidos al proveedor)
        public ObservableCollection<PurchaseOrderDetailDTO> CartItems { get; set; } = new();

        // Calcula el subtotal de los productos de la orden
        public decimal TotalOrder => CartItems.Sum(x => x.TotalCost);

        public void AddProduct(ProductDTO product)
        {
            // Verificar si el producto ya está en el carrito
            var existingProduct = CartItems.FirstOrDefault(x => x.IdProduct == product.IdProduct);
            if (existingProduct != null)
            {
                existingProduct.Quantity++;
            }
            else
            {
                CartItems.Add(new PurchaseOrderDetailDTO
                {
                    IdProduct = product.IdProduct,
                    ProductName = product.Description,
                    UnitPriceAtPurchase = product.PurchasePrice,
                    CurrentStock = product.Stock,
                    Quantity = 1
                });
            }
        }

        public void IncrementItem(PurchaseOrderDetailDTO item)
        {
            item.Quantity++;
        }

        public void DecrementItem(PurchaseOrderDetailDTO item)
        {
            if (item.Quantity > 1)
            {
                item.Quantity--;
            }
        }

        public void RemoveItem(PurchaseOrderDetailDTO item)
        {
            CartItems.Remove(item);
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }
    }
}
