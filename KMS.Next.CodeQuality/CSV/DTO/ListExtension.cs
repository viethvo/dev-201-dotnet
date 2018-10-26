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
        /// <param name = "resultList" > The list.</param>
        public static void PrintAll<T>(this List<T> resultList)
        {
            // Check type
            if (typeof(T) != typeof(Product) && typeof(T) != typeof(Category))
            {
                throw new Exception("This type is not supported!");
            }

            // Get properties of Category Class
            var properties = typeof(T).GetProperties();

            // Print title
            foreach (var property in properties)
            {
                Console.Write("{0}\t", property.Name);
            }

            Console.WriteLine();

            // Print content
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i] != null)
                {
                    foreach (var property in properties)
                    {
                        Console.Write("{0}\t", property.GetValue(resultList[i]));
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Line {0}: Invalid format", i + 1);
                }
            }
        }

        /// <summary>
        /// Writes content to csv file.
        /// </summary>
        /// <param name = "resultList" >The result list.</param>
        /// <param name = "path" >The path of csv file.</param>
        /// <return>Task.</return>
        public static async Task WriteToFile<T>(this List<T> resultList, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string content = string.Empty;

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
}
