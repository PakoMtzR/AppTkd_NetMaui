using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Utilities
{
    public class DatabaseConnection
    {
        // Método para obtener la ruta de la base de datos de manera multiplataforma
        public static string GetDatabasePath(string nameDB)
        {
            // Usamos FileSystem.AppDataDirectory de MAUI, que devuelve la ruta 
            // correcta para datos internos de la aplicación en cada plataforma.
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, nameDB);
        }
    }
}
