using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Utilities
{
    public class DatabaseConnection
    {
        // Método para obtener la ruta de la base de datos
        public static string GetDatabasePath(string nameDB)
        {
            string dbPath = string.Empty;
#if ANDROID
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            dbPath = System.IO.Path.Combine(folderPath, nameDB);
#elif IOS
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dbPath = System.IO.Path.Combine(folderPath, nameDB);
#elif WINDOWS
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            dbPath = System.IO.Path.Combine(folderPath, nameDB);
#endif
            return dbPath;
        }
    }
}
