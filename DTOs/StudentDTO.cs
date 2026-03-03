using System;
using System.ComponentModel.DataAnnotations;

namespace MauiApp1.DTOs
{
    public class StudentDTO
    {
        public int IdStudent { get; set; }
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

        // Foreign key IDs - these are necessary for mapping back to the Student model
        public int IdStudentOccupation { get; set; }
        public int IdStudentMaritalStatus { get; set; }
        public int IdStudentBelt { get; set; }

        // Descriptive properties for display in UI (optional, can be populated during mapping)
        public string? BeltColor { get; set; }
        public string? BeltDescription { get; set; }
        public string? MaritalStatusDescription { get; set; }
        public string? OccupationDescription { get; set; }
    }
}
