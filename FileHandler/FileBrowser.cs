using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileHandler
{
    public class FileBrowser
    {
        public List<string> ListOfReqFiles(string dir)
        {
            if (Directory.Exists(dir)) Debug.WriteLine($"[FileBrowser] {dir}, Exists!");
            else Debug.WriteLine($"[FileBrowser] {dir}, Doesn't Exists!");



                List<string> allNames = Directory.GetFiles(dir).Select(Path.GetFileName).ToList();
            allNames.RemoveAll(name => !_areThisFiles_Suitable(dir, name));
            return allNames;
        }

        private bool _areThisFiles_Suitable(string dir, string name)
        {
            string[] parts = name.Split('.');

            if (!name.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
                return false;

            string fullPath = Path.Combine(dir, name);
            return FileContainsExpectedHeader(fullPath);
        }

        private bool FileContainsExpectedHeader(string filePath) => true;

        //private bool FileContainsExpectedHeader(string filePath)
        //{
        //    try
        //    {
        //        // Регулярное выражение для поиска строки с прибором и датой
        //        // Допускает любые пробелы, дата в формате ДД.ММ.ГГГГ
        //        string pattern = @"прибором\s+ИТ-6\s+от\s+\d{2}\.\d{2}\.\d{4}";

        //        using (var reader = new StreamReader(filePath))
        //        {
        //            string line;
        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                if (Regex.IsMatch(line, pattern, RegexOptions.IgnoreCase))
        //                    return true;
        //            }
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }
}
