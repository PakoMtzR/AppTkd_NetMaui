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
                new Brand { IdBrand = 3, Name = "MOOTO" },
                new Brand { IdBrand = 4, Name = "DRAGONES ROJOS" }
            );

            // Datos de ejemplo para la tabla Product
            modelBuilder.Entity<Product>().HasData(
                new Product { IdProduct = 1, Description = "UNIFORME BLANCO KUP #0", SalePrice = 800m, PurchasePrice = 420m, Stock = 3, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 2, Description = "UNIFORME BLANCO KUP #1", SalePrice = 800m, PurchasePrice = 420m, Stock = 3, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 3, Description = "UNIFORME BLANCO KUP #2", SalePrice = 800m, PurchasePrice = 420m, Stock = 3, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 4, Description = "UNIFORME BLANCO KUP #3", SalePrice = 800m, PurchasePrice = 420m, Stock = 3, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 5, Description = "UNIFORME BLANCO KUP #4", SalePrice = 800m, PurchasePrice = 420m, Stock = 3, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 6, Description = "UNIFORME BLANCO KUP #5", SalePrice = 800m, PurchasePrice = 420m, Stock = 0, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 7, Description = "UNIFORME BLANCO KUP #6", SalePrice = 800m, PurchasePrice = 420m, Stock = 0, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 8, Description = "UNIFORME BLANCO KUP #7", SalePrice = 800m, PurchasePrice = 420m, Stock = 0, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 9, Description = "CINTA AMARILLA", SalePrice = 100m, PurchasePrice = 30m, Stock = 5, IdCategory = 2, IdBrand = 1},
                new Product { IdProduct = 10, Description = "CINTA NARANJA", SalePrice = 100m, PurchasePrice = 30m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 11, Description = "CINTA VERDE", SalePrice = 100m, PurchasePrice = 30m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 12, Description = "CINTA AZUL", SalePrice = 100m, PurchasePrice = 30m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 13, Description = "CINTA MARRÓN", SalePrice = 100m, PurchasePrice = 30m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 14, Description = "CINTA ROJA", SalePrice = 100m, PurchasePrice = 30m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 15, Description = "DISTINTIVO KUP VERDE", SalePrice = 50m, PurchasePrice = 20m, Stock = 5, IdCategory = 2, IdBrand = 1},
                new Product { IdProduct = 16, Description = "DISTINTIVO KUP AZUL", SalePrice = 50m, PurchasePrice = 20m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 17, Description = "DISTINTIVO KUP MARRÓN", SalePrice = 50m, PurchasePrice = 20m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 18, Description = "DISTINTIVO KUP ROJO", SalePrice = 50m, PurchasePrice = 20m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 19, Description = "DISTINTIVO DAN NEGRO", SalePrice = 50m, PurchasePrice = 20m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 20, Description = "UNIFORME PEQUEÑO TIGRE #000", SalePrice = 600m, PurchasePrice = 350m, Stock = 2, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 21, Description = "UNIFORME PEQUEÑO TIGRE #00", SalePrice = 600m, PurchasePrice = 350m, Stock = 2, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 22, Description = "UNIFORME PEQUEÑO TIGRE #0", SalePrice = 600m, PurchasePrice = 350m, Stock = 2, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 23, Description = "UNIFORME PEQUEÑO TIGRE #1", SalePrice = 600m, PurchasePrice = 350m, Stock = 2, IdCategory = 1, IdBrand = 1 },
                new Product { IdProduct = 24, Description = "CUELLO VERDE", SalePrice = 50m, PurchasePrice = 25m, Stock = 5, IdCategory = 2, IdBrand = 1},
                new Product { IdProduct = 25, Description = "CUELLO AZUL", SalePrice = 50m, PurchasePrice = 25m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 26, Description = "CUELLO MARRÓN", SalePrice = 50m, PurchasePrice = 25m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 27, Description = "CUELLO ROJO", SalePrice = 50m, PurchasePrice = 25m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 28, Description = "CUELLO ROJO/NEGRO", SalePrice = 50m, PurchasePrice = 25m, Stock = 5, IdCategory = 2, IdBrand = 1 },
                new Product { IdProduct = 29, Description = "PETO #0" , SalePrice = 800m, PurchasePrice = 400m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 30, Description = "PETO #1", SalePrice = 800m, PurchasePrice = 400m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 31, Description = "PETO #2", SalePrice = 800m, PurchasePrice = 400m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 32, Description = "PETO #3", SalePrice = 800m, PurchasePrice = 400m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 33, Description = "PETO #4", SalePrice = 800m, PurchasePrice = 400m, Stock = 0, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 34, Description = "PETO #5", SalePrice = 800m, PurchasePrice = 400m, Stock = 0, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 35, Description = "CASCO ABIERTO AZUL #S", SalePrice = 600m, PurchasePrice = 360, Stock = 2, IdCategory = 3, IdBrand = 1},
                new Product { IdProduct = 36, Description = "CASCO ABIERTO AZUL #M", SalePrice = 600m, PurchasePrice = 360, Stock = 2, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 37, Description = "CASCO ABIERTO AZUL #L", SalePrice = 600m, PurchasePrice = 360, Stock = 2, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 38, Description = "CASCO ABIERTO ROJO #S", SalePrice = 600m, PurchasePrice = 360, Stock = 2, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 39, Description = "CASCO ABIERTO ROJO #M", SalePrice = 600m, PurchasePrice = 360, Stock = 2, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 40, Description = "CASCO ABIERTO ROJO #L", SalePrice = 600m, PurchasePrice = 360, Stock = 2, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 41, Description = "ESPINILLERA C/EMPEINERA NEGRA #XS", SalePrice = 400m, PurchasePrice = 280m, Stock = 3, IdCategory = 3, IdBrand = 1},
                new Product { IdProduct = 42, Description = "ESPINILLERA C/EMPEINERA NEGRA #S", SalePrice = 400m, PurchasePrice = 280m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 43, Description = "ESPINILLERA C/EMPEINERA NEGRA #M", SalePrice = 400m, PurchasePrice = 280m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 44, Description = "ESPINILLERA C/EMPEINERA NEGRA #L", SalePrice = 400m, PurchasePrice = 280m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 45, Description = "ESPINILLERA C/EMPEINERA NEGRA #XL", SalePrice = 400m, PurchasePrice = 280m, Stock = 0, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 46, Description = "CODERA NEGRA #XS", SalePrice = 400m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 47, Description = "CODERA NEGRA #S", SalePrice = 400m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 48, Description = "CODERA NEGRA #M", SalePrice = 400m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 49, Description = "CODERA NEGRA #L", SalePrice = 400m, PurchasePrice = 250m, Stock = 0, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 50, Description = "CODERA NEGRA #XL", SalePrice = 400m, PurchasePrice = 250m, Stock = 0, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 51, Description = "CONCHA P/HOMBRE #XS", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1},
                new Product { IdProduct = 52, Description = "CONCHA P/HOMBRE #S", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 53, Description = "CONCHA P/HOMBRE #M", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 54, Description = "CONCHA P/HOMBRE #L", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 55, Description = "CONCHA P/HOMBRE #XL", SalePrice = 450m, PurchasePrice = 250m, Stock = 0, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 56, Description = "CONCHA P/MUJER #XS", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 57, Description = "CONCHA P/MUJER #S", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 58, Description = "CONCHA P/MUJER #M", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 59, Description = "CONCHA P/MUJER #L", SalePrice = 450m, PurchasePrice = 250m, Stock = 3, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 60, Description = "CONCHA P/MUJER #XL", SalePrice = 450m, PurchasePrice = 250m, Stock = 0, IdCategory = 3, IdBrand = 1 },
                new Product { IdProduct = 61, Description = "BACK PACK AZUL", SalePrice = 550m, PurchasePrice = 350m, Stock = 2, IdCategory = 6, IdBrand = 1 },
                new Product { IdProduct = 62, Description = "BACK PACK ROJO", SalePrice = 550m, PurchasePrice = 350m, Stock = 2, IdCategory = 6, IdBrand = 1 },
                new Product { IdProduct = 63, Description = "PLAYERA DRAGONES ROJOS", SalePrice= 500m, PurchasePrice = 250m, Stock = 5, IdCategory = 4, IdBrand = 4 }
            );
        }
    }
}
