using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Models;
using MauiApp1.Utilities;

namespace MauiApp1.Data
{
    public class DatabaseContext : DbContext
    {
        // Definición de los DbSet para cada entidad
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentOccupation> StudentOccupations { get; set; }
        public DbSet<StudentMaritalStatus> StudentMaritalStatuses { get; set; }
        public DbSet<StudentBelt> StudentBelts { get; set; }

        // Configuración de la conexión a la base de datos SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = $"Filename={DatabaseConnection.GetDatabasePath("taekwondo.db")}";
            optionsBuilder.UseSqlite(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de Student - Occupation (Uno a Muchos)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.StudentOccupation)
                .WithMany(o => o.Students)
                .HasForeignKey(s => s.IdStudentOccupation)
                .OnDelete(DeleteBehavior.Restrict);  // No cascada, pero obligatoria

            // Configuración de Student - Belt (Uno a Muchos)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.StudentBelt)
                .WithMany(b => b.Students)
                .HasForeignKey(s => s.IdStudentBelt)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Student - MaritalStatus (Uno a Muchos)
            modelBuilder.Entity<Student>()  
                .HasOne(s => s.StudentMaritalStatus)
                .WithMany(m => m.Students)
                .HasForeignKey(s => s.IdStudentMaritalStatus)
                .OnDelete(DeleteBehavior.Restrict);

            // Datos de las tablas de referencia
            modelBuilder.Entity<StudentOccupation>().HasData(
                new StudentOccupation { IdStudentOccupation = 1, Description = "Estudiante" },
                new StudentOccupation { IdStudentOccupation = 2, Description = "Trabajador" },
                new StudentOccupation { IdStudentOccupation = 3, Description = "Desempleado" },
                new StudentOccupation { IdStudentOccupation = 4, Description = "Jubilado" },
                new StudentOccupation { IdStudentOccupation = 5, Description = "Independiente" }
            );

            modelBuilder.Entity<StudentMaritalStatus>().HasData(
                new StudentMaritalStatus { IdStudentMaritalStatus = 1, Description = "Soltero/a" },
                new StudentMaritalStatus { IdStudentMaritalStatus = 2, Description = "Casado/a" },
                new StudentMaritalStatus { IdStudentMaritalStatus = 3, Description = "Divorciado/a" },
                new StudentMaritalStatus { IdStudentMaritalStatus = 4, Description = "Viudo/a" },
                new StudentMaritalStatus { IdStudentMaritalStatus = 5, Description = "Union Libre" },
                new StudentMaritalStatus { IdStudentMaritalStatus = 6, Description = "Separado/a" }
            );

            modelBuilder.Entity<StudentBelt>().HasData(
                new StudentBelt { IdStudentBelt = 1, Color = "Blanca", Description = "Principiante" },
                new StudentBelt { IdStudentBelt = 2, Color = "Amarilla", Description = "10° Kup" },
                new StudentBelt { IdStudentBelt = 3, Color = "Naranja", Description = "9° Kup" },
                new StudentBelt { IdStudentBelt = 4, Color = "Naranja Avanzada", Description = "8° Kup" },
                new StudentBelt { IdStudentBelt = 5, Color = "Verde", Description = "7° Kup" },
                new StudentBelt { IdStudentBelt = 6, Color = "Verde Avanzada", Description = "6° Kup" },
                new StudentBelt { IdStudentBelt = 7, Color = "Azul", Description = "5° Kup" },
                new StudentBelt { IdStudentBelt = 8, Color = "Azul Avanzada", Description = "4° Kup" },
                new StudentBelt { IdStudentBelt = 9, Color = "Marrón", Description = "3° Kup" },
                new StudentBelt { IdStudentBelt = 10, Color = "Marrón Avanzada", Description = "2° Kup" },
                new StudentBelt { IdStudentBelt = 11, Color = "Roja", Description = "1° Kup" },
                new StudentBelt { IdStudentBelt = 12, Color = "Roja Avanzada", Description = "IEBY Poom" },
                new StudentBelt { IdStudentBelt = 13, Color = "Roja Avanzada", Description = "IEBY Dan" },
                new StudentBelt { IdStudentBelt = 14, Color = "Negra", Description = "1° Poom" },
                new StudentBelt { IdStudentBelt = 15, Color = "Negra", Description = "2° Poom" },
                new StudentBelt { IdStudentBelt = 16, Color = "Negra", Description = "3° Poom" },
                new StudentBelt { IdStudentBelt = 17, Color = "Negra", Description = "4° Poom" },
                new StudentBelt { IdStudentBelt = 18, Color = "Negra", Description = "1° Dan" },
                new StudentBelt { IdStudentBelt = 19, Color = "Negra", Description = "2° Dan" },
                new StudentBelt { IdStudentBelt = 20, Color = "Negra", Description = "3° Dan" },
                new StudentBelt { IdStudentBelt = 21, Color = "Negra", Description = "4° Dan" },
                new StudentBelt { IdStudentBelt = 22, Color = "Negra", Description = "5° Dan" },
                new StudentBelt { IdStudentBelt = 23, Color = "Negra", Description = "6° Dan" },
                new StudentBelt { IdStudentBelt = 24, Color = "Negra", Description = "7° Dan" },
                new StudentBelt { IdStudentBelt = 25, Color = "Negra", Description = "8° Dan" },
                new StudentBelt { IdStudentBelt = 26, Color = "Negra", Description = "9° Dan" }
            );

        }

    }
}
