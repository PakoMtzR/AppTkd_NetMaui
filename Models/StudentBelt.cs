using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    public class StudentBelt
    {
        [Key]
        public int IdStudentBelt { get; set; }

        [Required]
        [MaxLength(50)]
        public string Color { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        // Propiedad de navegación para la relación con Student
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
