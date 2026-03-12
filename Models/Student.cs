using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.Models
{
    // Clase 'Alumno': Informacion general de los alumnos.
    public class Student
    {
        [Key]
        public int IdStudent { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? PaternalSurname { get; set; }
        public string? MaternalSurname { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public bool IsActive { get; set; }
        public int? Code { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }

        // Llaves Foraneas
        public int IdStudentOccupation { get; set; }
        public int IdStudentMaritalStatus { get; set; }
        public int IdStudentBelt { get; set; }

        // Propiedades de navegación
        public StudentOccupation? StudentOccupation { get; set; }
        public StudentMaritalStatus? StudentMaritalStatus { get; set; }
        public StudentBelt? StudentBelt { get; set; }
    }
}
