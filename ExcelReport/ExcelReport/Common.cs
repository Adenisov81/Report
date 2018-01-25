using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReport
{
    public static class Common
    {
        public static string GetNextAvailableFilename(string filename)
        {
            if (!System.IO.File.Exists(filename)) return filename;

            string alternateFilename;
            int fileNameIndex = 1;
            do
            {
                fileNameIndex += 1;
                alternateFilename = CreateNumberedFilename(filename, fileNameIndex);
            } while (System.IO.File.Exists(alternateFilename));

            return alternateFilename;
        }

        private static string CreateNumberedFilename(string filename, int number)
        {
            string plainName = System.IO.Path.GetFileNameWithoutExtension(filename);
            string extension = System.IO.Path.GetExtension(filename);
            return string.Format("{0}{1}{2}", plainName, number, extension);
        }

        public static string GetColumnLetter(string colNumber)
        {
            if (string.IsNullOrEmpty(colNumber))
            {
                throw new ArgumentNullException(colNumber);
            }

            string colName = String.Empty;

            try
            {
                var colNum = Convert.ToInt32(colNumber);
                var mod = colNum % 26;
                var div = Math.Floor((double)(colNum) / 26);
                colName = ((div > 0) ? GetColumnLetter((div - 1).ToString()) : String.Empty) + Convert.ToChar(mod + 65);
            }
            finally
            {
                colName = colName == String.Empty ? "A" : colName;
            }

            return colName;
        }
    }
}
