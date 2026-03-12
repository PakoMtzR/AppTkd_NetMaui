using MauiApp1.Data;
using MauiApp1.Models;
using MauiApp1.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class PurchaseService
    {
        private readonly DatabaseContext _context;

        public PurchaseService(DatabaseContext context)
        {
            _context = context;
        }

        // --- Mappers ---
        private PurchaseOrderDTO MapToDTO(PurchaseOrder order)
        {
            if (order == null) return null;
            return new PurchaseOrderDTO
            {
                IdPurchaseOrder = order.IdPurchaseOrder,
                PurchaseOrderNumber = order.PurchaseOrderNumber,
                RegistrationDate = order.RegistrationDate,
                OrderCost = order.OrderCost,
                ShipmentCost = order.ShipmentCost,
                PurchaseOrderDetails = order.PurchaseOrderDetails.Select(sd => MapToDetailDTO(sd)).ToList()
            };
        }

        private PurchaseOrderDetailDTO MapToDetailDTO(PurchaseOrderDetail detail)
        {
            if (detail == null) return null;
            return new PurchaseOrderDetailDTO
            {
                IdPurchaseOrderDetail = detail.IdPurchaseOrderDetail,
                Quantity = detail.Quantity,
                IdPurchaseOrder = detail.IdPurchaseOrder,
                IdProduct = detail.IdProduct,
                ProductName = detail.Product?.Description,
                UnitPriceAtPurchase = detail.UnitPriceAtPurchase,
                CurrentStock = detail.Product?.Stock ?? 0
            };
        }
        // --- End Mappers ---

        public async Task<List<PurchaseOrderDTO>> GetPurchaseHistory()
        {
            var orders = await _context.PurchaseOrders
                .Include(o => o.PurchaseOrderDetails)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.RegistrationDate)
                .ToListAsync();

            return orders.Select(o => MapToDTO(o)).ToList();
        }

        public async Task<PurchaseOrderDTO?> GetPurchaseById(int idPurchase)
        {
            var order = await _context.PurchaseOrders
                .Include(o => o.PurchaseOrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.IdPurchaseOrder == idPurchase);

            return MapToDTO(order!);
        }

        public async Task<bool> SavePurchase(PurchaseOrderDTO orderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new PurchaseOrder
                {
                    PurchaseOrderNumber = orderDto.PurchaseOrderNumber,
                    RegistrationDate = DateTime.Now,
                    OrderCost = orderDto.OrderCost,
                    ShipmentCost = orderDto.ShipmentCost
                };

                foreach (var detailDto in orderDto.PurchaseOrderDetails)
                {
                    var product = await _context.Products.FindAsync(detailDto.IdProduct);
                    if (product == null) throw new Exception("Producto no encontrado");

                    // Sumar el stock recibido del proveedor
                    product.Stock += detailDto.Quantity;
                    _context.Products.Update(product);

                    var detail = new PurchaseOrderDetail
                    {
                        IdProduct = detailDto.IdProduct,
                        Quantity = detailDto.Quantity,
                        UnitPriceAtPurchase = detailDto.UnitPriceAtPurchase
                    };
                    order.PurchaseOrderDetails.Add(detail);
                }

                _context.PurchaseOrders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public string GeneratePurchaseOrderNumber()
        {
            return "P-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}
