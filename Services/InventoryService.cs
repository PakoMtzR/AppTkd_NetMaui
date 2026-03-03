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
    }
}
