using MauiApp1.Data;
using MauiApp1.Models;
using MauiApp1.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class InventoryService
    {
        private readonly DatabaseContext _context;

        public InventoryService(DatabaseContext context)
        {
            _context = context;
        }

        // --- Mappers ---
        // Mapea un modelo Category a un CategoryDTO
        private CategoryDTO MapToDTO(Category category)
        {
            if (category == null) return null;
            return new CategoryDTO
            {
                IdCategory = category.IdCategory,
                Description = category.Description
            };
        }

        // Mapea un CategoryDTO a un modelo Category
        private Category MapToModel(CategoryDTO dto)
        {
            if (dto == null) return null;
            return new Category
            {
                IdCategory = dto.IdCategory,
                Description = dto.Description
            };
        }

        // Mapea un modelo Product a un ProductDTO
        private ProductDTO MapToDTO(Product product)
        {
            if (product == null) return null;
            return new ProductDTO
            {
                IdProduct = product.IdProduct,
                Description = product.Description,
                SalePrice = product.SalePrice,
                PurchasePrice = product.PurchasePrice,
                Stock = product.Stock,
                IdCategory = product.IdCategory,
                IdBrand = product.IdBrand,
                CategoryDescription = product.Category?.Description, // Usar operador null-conditional
                BrandName = product.Brand?.Name // Usar operador null-conditional
            };
        }

        // Mapea un ProductDTO a un modelo Product
        private Product MapToModel(ProductDTO dto)
        {
            if (dto == null) return null;
            return new Product
            {
                IdProduct = dto.IdProduct,
                Description = dto.Description,
                SalePrice = dto.SalePrice,
                PurchasePrice = dto.PurchasePrice,
                Stock = dto.Stock,
                IdCategory = dto.IdCategory,
                IdBrand = dto.IdBrand
            };
        }

        // Mapea un modelo Brand a un BrandDTO
        private BrandDTO MapToBrandDTO(Brand brand)
        {
            if (brand == null) return null;
            return new BrandDTO
            {
                IdBrand = brand.IdBrand,
                Name = brand.Name
            };
        }
        // --- End Mappers ---


        // Obtiene todas las categorías
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(c => MapToDTO(c)).ToList();
        }

        // Obtiene una categoría por su ID
        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return MapToDTO(category);
        }

        // Añade una nueva categoría
        public async Task<CategoryDTO> AddCategory(CategoryDTO dto)
        {
            var category = MapToModel(dto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            dto.IdCategory = category.IdCategory; // Actualiza el DTO con el ID generado por la BD
            return dto;
        }

        // Actualiza una categoría existente
        public async Task UpdateCategory(CategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(dto.IdCategory);
            if (category != null)
            {
                category.Description = dto.Description;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
        }

        // Elimina una categoría por su ID
        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        // Obtiene todas las marcas
        public async Task<List<BrandDTO>> GetAllBrands()
        {
            var brands = await _context.Brands.ToListAsync();
            return brands.Select(b => MapToBrandDTO(b)).ToList();
        }

        // Obtiene todos los productos
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var products = await _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.Brand)
                                        .ToListAsync();
            return products.Select(p => MapToDTO(p)).ToList();
        }

        // Obtiene un producto por su ID
        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.Brand)
                                        .FirstOrDefaultAsync(p => p.IdProduct == id);
            return MapToDTO(product);
        }

        // Añade un nuevo producto
        public async Task<ProductDTO> AddProduct(ProductDTO dto)
        {
            var product = MapToModel(dto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            dto.IdProduct = product.IdProduct; // Actualiza el DTO con el ID generado por la BD
            return dto;
        }

        // Actualiza un producto existente
        public async Task UpdateProduct(ProductDTO dto)
        {
            var product = await _context.Products.FindAsync(dto.IdProduct);
            if (product != null)
            {
                product.Description = dto.Description;
                product.SalePrice = dto.SalePrice;
                product.PurchasePrice = dto.PurchasePrice;
                product.Stock = dto.Stock;
                product.IdCategory = dto.IdCategory;
                product.IdBrand = dto.IdBrand;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }

        // Elimina un producto por su ID
        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
