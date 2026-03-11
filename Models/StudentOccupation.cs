using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    // Clase 'Ocupacion': Almacena la ocupacion de los alumnos.
    public class StudentOccupation
    {
        [Key]
        public int IdStudentOccupation { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        // Propiedad de navegación para la relación con 'Student'
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
