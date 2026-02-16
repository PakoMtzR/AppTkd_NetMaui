using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    public class StudentOccupation
    {
        [Key]
        public int IdStudentOccupation { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        // Propiedadd de navegación para la relación con Student
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
