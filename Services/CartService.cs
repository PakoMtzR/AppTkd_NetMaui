using MauiApp1.DTOs;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiApp1.Services
{
    public class CartService
    {
        // Lista de productos en el carrito, se enlaza directamente con la UI a través del ViewModel
        public ObservableCollection<SaleDetailDTO> CartItems { get; set; } = new ObservableCollection<SaleDetailDTO>();

        // Calcula el total del carrito sumando el total de cada item
        public decimal TotalCart => CartItems.Sum(x => x.TotalPrice);

        public void AddProduct(ProductDTO product)
        {
            // Verificar si el producto ya está en el carrito
            var existingProduct = CartItems.FirstOrDefault(x => x.IdProduct == product.IdProduct);
            if (existingProduct != null)
            {
                // Solo sumar una unidad si no excede el stock total
                int newQuantity = existingProduct.Quantity + 1;
                if (newQuantity <= product.Stock)
                {
                    existingProduct.Quantity = newQuantity;
                }
            }
            else
            {
                CartItems.Add(new SaleDetailDTO
                {
                    IdProduct = product.IdProduct,
                    ProductName = product.Description,
                    UnitPriceAtSale = product.SalePrice,
                    AvailableStock = product.Stock,
                    Quantity = 1
                });
            }
        }

        public bool IncrementItem(SaleDetailDTO item)
        {
            if (item.Quantity < item.AvailableStock)
            {
                item.Quantity++;
                return true;
            }
            return false;
        }

        public void DecrementItem(SaleDetailDTO item)
        {
            if (item.Quantity > 1) item.Quantity--;
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
