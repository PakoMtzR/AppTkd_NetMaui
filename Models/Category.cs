using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    // Clase Categoria: Esta clase tiene como objetivo filtrar los productos.
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        // Propiedad de navegación para la relación con 'Product'
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
