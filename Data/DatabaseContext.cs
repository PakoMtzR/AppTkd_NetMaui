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
        // Tablas relacionadas con "Alumnos"
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentOccupation> StudentOccupations { get; set; }
        public DbSet<StudentMaritalStatus> StudentMaritalStatuses { get; set; }
        public DbSet<StudentBelt> StudentBelts { get; set; }
        // Tablas relacionadas con "Inventario/Ventas"
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        // Configuración de la conexión a la base de datos SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = $"Filename={DatabaseConnection.GetDatabasePath("taekwondo.db")}";
            optionsBuilder.UseSqlite(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuracion de las Relaciones 
            // Configuración de Relaciones para Alumnos
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

            // Configuración de las relaciones para Inventario/Ventas
            // Configuración de Product - Category (Uno a Muchos)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.IdCategory)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Product - Brand (Uno a Muchos)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.IdBrand)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de SaleDetail - Sale (Uno a Muchos)
            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Sale)
                .WithMany(s => s.SaleDetails)
                .HasForeignKey(sd => sd.IdSale)
                .OnDelete(DeleteBehavior.Cascade);  // Cascada para eliminar detalles si se elimina la venta

            // Configuración de SaleDetail - Product (Uno a Muchos)
            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Product)
                .WithMany(p => p.SaleDetails)
                .HasForeignKey(sd => sd.IdProduct)
                .OnDelete(DeleteBehavior.Restrict);  // No cascada, pero obligatoria

            // Configuración de PurchaseOrderDetail - PurchaseOrder (Uno a Muchos)
            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.IdPurchaseOrder)
                .OnDelete(DeleteBehavior.Cascade);  // Cascada para eliminar detalles si se elimina la orden de compra

            // Configuración de PurchaseOrderDetail - Product (Uno a Muchos)
            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.Product)
                .WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.IdProduct)
                .OnDelete(DeleteBehavior.Restrict);  // No cascada, pero obligatoria

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

            modelBuilder.Entity<Category>().HasData(
                new Category { IdCategory = 1, Description = "Uniformes" },
                new Category { IdCategory = 2, Description = "Cintas, Distintivos y Cuellos" },
                new Category { IdCategory = 3, Description = "Protecciones" },
                new Category { IdCategory = 4, Description = "Ropa" },
                new Category { IdCategory = 5, Description = "Tenis" },
                new Category { IdCategory = 6, Description = "Mochilas" }
            );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { IdBrand = 1, Name = "MOONDO"},
                new Brand { IdBrand = 2, Name = "KOS" },
                new Brand { IdBrand = 3, Name = "MOOTO" }
            );
        }

    }
}
