using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    public class StudentMaritalStatus
    {
        [Key]
        public int IdStudentMaritalStatus { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        // Propiedad de navegación para la relación con 'Student'
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
