using MauiApp1.DTOs;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Services
{
    public class CartService
    {
        public ObservableCollection<SaleDetailDTO> CartItems { get; set; } = new ObservableCollection<SaleDetailDTO>();

        public decimal TotalCart => CartItems.Sum(x => x.TotalPrice);

        public void AddProduct(ProductDTO product, int quantity)
        {
            var existing = CartItems.FirstOrDefault(x => x.IdProduct == product.IdProduct);
            if (existing != null)
            {
                existing.Quantity += quantity;
                existing.TotalPrice = existing.Quantity * product.SalePrice;
                
                // Actualizar la referencia para disparar cambios en la UI si es necesario
                var index = CartItems.IndexOf(existing);
                CartItems[index] = existing;
            }
            else
            {
                CartItems.Add(new SaleDetailDTO
                {
                    IdProduct = product.IdProduct,
                    ProductName = product.Description,
                    ProductPrice = product.SalePrice,
                    Quantity = quantity,
                    TotalPrice = quantity * product.SalePrice
                });
            }
        }

        public void RemoveItem(SaleDetailDTO item)
        {
            CartItems.Remove(item);
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }
    }
}
