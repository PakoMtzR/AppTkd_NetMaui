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

        // TODO: Agregar validación para no exceder el stock disponible al añadir productos o incrementar cantidades
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
                    existingProduct.TotalPrice = existingProduct.Quantity * product.SalePrice;
                }
            }
            else
            {
                CartItems.Add(new SaleDetailDTO
                {
                    IdProduct = product.IdProduct,
                    ProductName = product.Description,
                    ProductPrice = product.SalePrice,
                    AvailableStock = product.Stock, // Guardamos el stock disponible
                    Quantity = 1,
                    TotalPrice = product.SalePrice
                });
            }
        }

        public bool IncrementItem(SaleDetailDTO item)
        {
            if (item.Quantity < item.AvailableStock)
            {
                item.Quantity++;
                item.TotalPrice = item.Quantity * item.ProductPrice;
                return true;
            }
            return false;
        }

        public void DecrementItem(SaleDetailDTO item)
        {
            if (item.Quantity > 1)
            {
                item.Quantity--;
                item.TotalPrice = item.Quantity * item.ProductPrice;
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
