using MauiApp1.Data;
using MauiApp1.Models;
using MauiApp1.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MauiApp1.Services
{
    public class SalesService
    {
        private readonly DatabaseContext _context;

        public SalesService(DatabaseContext context)
        {
            _context = context;
        }

        // --- Mappers ---
        private SaleDTO MapToDTO(Sale sale)
        {
            if (sale == null) return null;
            return new SaleDTO
            {
                IdSale = sale.IdSale,
                SaleNumber = sale.SaleNumber,
                ClientName = sale.ClientName,
                RegistrationDate = sale.RegistrationDate,
                TotalPrice = sale.TotalPrice,
                SaleDetails = sale.SaleDetails.Select(sd => MapToDetailDTO(sd)).ToList()
            };
        }

        private SaleDetailDTO MapToDetailDTO(SaleDetail detail)
        {
            if (detail == null) return null;
            return new SaleDetailDTO
            {
                IdSaleDetail = detail.IdSaleDetail,
                Quantity = detail.Quantity,
                IdSale = detail.IdSale,
                IdProduct = detail.IdProduct,
                ProductName = detail.Product?.Description,
                UnitPriceAtSale = detail.UnitPriceAtSale // El DTO calculará TotalPrice
            };
        }
        // --- End Mappers ---

        public async Task<List<SaleDTO>> GetSalesHistory()
        {
            var sales = await _context.Sales
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .OrderByDescending(s => s.RegistrationDate)
                .ToListAsync();

            return sales.Select(s => MapToDTO(s)).ToList();
        }

        public async Task<SaleDTO?> GetSaleById(int idSale)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.IdSale == idSale);

            return MapToDTO(sale!);
        }

        public async Task<bool> SaveSale(SaleDTO saleDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sale = new Sale
                {
                    SaleNumber = saleDto.SaleNumber,
                    ClientName = saleDto.ClientName,
                    RegistrationDate = DateTime.Now,
                    TotalPrice = saleDto.TotalPrice
                };

                foreach (var detailDto in saleDto.SaleDetails)
                {
                    var product = await _context.Products.FindAsync(detailDto.IdProduct);
                    if (product == null || product.Stock < detailDto.Quantity)
                    {
                        throw new Exception("Stock insuficiente");
                    }

                    product.Stock -= detailDto.Quantity;
                    _context.Products.Update(product);

                    var detail = new SaleDetail
                    {
                        IdProduct = detailDto.IdProduct,
                        Quantity = detailDto.Quantity,
                        UnitPriceAtSale = detailDto.UnitPriceAtSale
                    };
                    sale.SaleDetails.Add(detail);
                }

                _context.Sales.Add(sale);
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

        public string GenerateSaleNumber()
        {
            return "S-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}
