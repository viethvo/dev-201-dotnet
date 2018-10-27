using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KMS.Next.CodeQuality.CSV.DTO
{
    /// <summary>
    /// Class ListExtension.
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Prints all content from list.
        /// </summary>
        /// <param name = "resultList">The list.</param>
        public static void PrintAll<T>(this List<T> resultList)
        {
            // Check type
            if (typeof(T) != typeof(Product) && typeof(T) != typeof(Category))
            {
                throw new Exception("This type is not supported!");
            }

            // Get properties of Category Class
            var properties = typeof(T).GetProperties();

            // Add title
            string[] title = new string[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                title[i] = properties[i].Name;
            }

            // Add content
            List<string[]> content = new List<string[]>();

            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i] == null)
                {
                    content.Add(null);
                    continue;
                }

                string[] row = new string[properties.Length];

                for (int j = 0; j < properties.Length; j++)
                {
                    bool isNull = properties[j].GetValue(resultList[i]) != null ? false : true;
                    row[j] = isNull ? string.Empty : properties[j].GetValue(resultList[i]).ToString();
                }
                content.Add(row);
            }

            ConsoleTable.PrintTable(title, content);
        }

        /// <summary>
        /// Writes content to csv file.
        /// </summary>
        /// <param name = "resultList">The result list.</param>
        /// <param name = "path">The path of csv file.</param>
        /// <return>Task.</return>
        public static async Task WriteToFile<T>(this List<T> resultList, string path)
        {
            // Check file
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Get properties
            var properties = typeof(T).GetProperties();

            // Write file
            var writer = File.OpenWrite(path);
            var streamWriter = new StreamWriter(writer);

            foreach (var item in resultList)
            {
                await streamWriter.WriteLineAsync(item.ToString());
            }

            // Close stream
            streamWriter.Close();
            writer.Close();
        }
    }

    /// <summary>
    /// Class ConsoleTable.
    /// </summary>
    public static class ConsoleTable
    {
        /// <summary>
        /// Gets or sets table width.
        /// </summary>
        /// <value>The table width.</value>
        public static int tableWidth = 150;

        /// <summary>
        /// Draws console table.
        /// </summary>
        /// <param name = "title">The title of table.</param>
        /// <param name = "content">The content of table.</param>
        /// <return>Void.</return>
        public static void PrintTable(string[] title, List<string[]> content)
        {
            PrintBorderLine();
            PrintRow(title);
            PrintBorderLine();

            for (int i = 0; i < content.Count; i++)
            {
                if (content[i] != null)
                {
                    PrintRow(content[i]);
                }
                else
                {
                    PrintRow(content[i], i + 1);
                }
            }
            PrintBorderLine();
        }

        /// <summary>
        /// Prints border line.
        /// </summary>
        /// <return>void.</return>
        private static void PrintBorderLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        /// <summary>
        /// Prints table row.
        /// </summary>
        /// <param name = "columns">The column of values.</param>
        /// <param name = "errorLine">The error line.</param>
        /// <return>Void.</return>
        private static void PrintRow(string[] columns, int errorLine = -1)
        {
            if (columns == null && errorLine != -1)
            {
                Console.WriteLine("Line {0}: Invalid format", errorLine);
                return;
            }

            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        /// <summary>
        /// Align text cell center.
        /// </summary>
        /// <param name = "text">The cell value.</param>
        /// <param name = "width">The table width.</param>
        /// <return>Void.</return>
        private static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}


