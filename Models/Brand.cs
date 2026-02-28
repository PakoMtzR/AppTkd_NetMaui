using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    public class Brand
    {
        [Key]
        public int IdBrand { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        // Propiedad de navegación para la relación con 'Product'
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
