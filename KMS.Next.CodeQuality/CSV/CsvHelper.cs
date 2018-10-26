using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Next.CodeQuality.CSV
{
    /// <summary>
    /// Class CsvHelper.
    /// </summary>
    public class CsvHelper
    {
        /// <summary>
        /// Reads catefory from csv file.
        /// </summary>
        /// <param name = "path">Path of category csv file.</param>
        /// <return>List of T</return>
        public static List<T> ReadFromFile<T>(string path)
        {
            // Check path
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            // Check type
            if (typeof(T) != typeof(Category) && typeof(T) != typeof(Product))
            {
                throw new Exception("This type is not supported!");
            }

            // Read all from file
            var lines = ReadAllLines(path);

            // Convert lines to list
            var resultList = ConvertToList<T>(lines);
            return resultList;
        }

        /// <summary>
        /// Exports file map between category and product.
        /// </summary>
        /// <param name = "listCategory">List of category.</param>
        /// <param name = "listProduct">List of product.</param>
        /// <param name = "path">Path of export file.</param>
        /// <return>Task.</return>
        public static async Task ExportFileMapBetween(List<Category> listCategory, List<Product> listProduct, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var mapList = Map(listCategory, listProduct);
            string content = ConvertListMapToString(mapList);

            // Write file
            var writer = File.OpenWrite(path);
            var streamWriter = new StreamWriter(writer);
            await streamWriter.WriteAsync(content);

            // Close stream
            streamWriter.Close();
            writer.Close();
        }

        /// <summary>
        /// Gets list map between category and product.
        /// </summary>
        /// <param name = "listCategory">List of category.</param>
        /// <param name = "listProduct">List of product.</param>
        /// <return>System.String.</return>
        private static Dictionary<string, string> Map(List<Category> listCategory, List<Product> listProduct)
        {
            int total = listProduct.Count; // total products
            int totalCID = 0; // total products which have category ID
            
            // Create result list map
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("CategoryName", "ProductCount");

            foreach (var category in listCategory)
            {
                int count = listProduct.FindAll(p => p.CategoryId == category.CategoryId).Count;
                if (count != 0)
                {
                    totalCID += count;
                    result.Add(category.CategoryName, count.ToString());
                }
            }

            int other = totalCID < total ? total - totalCID : 0;

            // Count for others
            if (other != 0)
            {
                result.Add("Others", other.ToString());
            }

            return result;
        }

        /// <summary>
        /// Converts list map to string.
        /// </summary>
        /// <param name = "listMap">List map between category and product.</param>
        /// <return>System.String.</return>
        private static string ConvertListMapToString(Dictionary<string, string> listMap)
        {
            StringBuilder builder = new StringBuilder();

            foreach(var map in listMap)
            {
                string temp = string.Format("{0},{1}", map.Key, map.Value);
                builder.AppendLine(temp);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Converts content of csv file to list.
        /// </summary>
        /// <param name = "lines">Content of csv file.</param>
        /// <return>List of T</return>
        private static List<T> ConvertToList<T>(Task<string[]> lines)
        {
            bool isHeader = true;
            var propListT = typeof(T).GetProperties();
            var listResult = new List<T>();

            foreach (string line in lines.Result)
            {
                // Skip header
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }
                // Check line if empty
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                // Get values of line
                var values = line.Split(',');

                // Check if values match properties
                if (values.Length != propListT.Length)
                {
                    listResult.Add(default(T));
                    continue;
                }

                var item = Activator.CreateInstance(typeof(T));
                try
                {
                    // Add values to class if match
                    for (int i = 0; i < propListT.Length; i++)
                    {
                        if (string.IsNullOrEmpty(values[i]))
                        {
                            continue;
                        }
                        // Check if nullable and match
                        Type type = Nullable.GetUnderlyingType(propListT[i].PropertyType) ?? propListT[i].PropertyType;
                        object safeValue = (values[i] == null) ? null : Convert.ChangeType(values[i], type);
                        propListT[i].SetValue(item, safeValue, null);
                    }
                }
                catch
                {
                    // Can't convert type
                    listResult.Add(default(T));
                    continue;
                }

                // Convert type success and add to list
                var itemConvert = (T)Convert.ChangeType(item, typeof(T));
                listResult.Add(itemConvert);
            }
            return listResult;
        }

        /// <summary>
        /// Reads content of csv file.
        /// </summary>
        /// <param name = "path">Path of category csv file.</param>
        /// <return>List of T</return>
        private static async Task<string[]> ReadAllLines(string path)
        {
            StreamReader reader = null;
            string fileText = string.Empty;
            try
            {
                reader = File.OpenText(path);
                fileText = await reader.ReadToEndAsync();
            }
            catch
            {
                throw new Exception("Failed to read file");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }

    /// <summary>
    /// Class Product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets identifier of product.
        /// </summary>
        /// <value>The identifier of product.</value>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets name of product.
        /// </summary>
        /// <value>The name of product.</value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets price of product.
        /// </summary>
        /// <value>The price of product.</value>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets description of product.
        /// </summary>
        /// <value>The description of product.</value>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets expired date of product.
        /// </summary>
        /// <value>The expired date of product.</value>
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Gets or sets category identifier of product.
        /// </summary>
        /// <value>The category identifier of product.</value>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets deleted flag.
        /// </summary>
        /// <value>The deleted flag.</value>
        public bool DeletedFlag { get; set; }

        /// <summary>
        /// Gets and convert Product class to string.
        /// </summary>
        /// <return>System.String.</return>
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", ProductId, ProductName, Price, ProductDescription, ExpiredDate, CategoryId, DeletedFlag);
        }
    }

    /// <summary>
    /// Class Category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets identifier of category .
        /// </summary>
        /// <value>The identifier of category.</value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets name of category .
        /// </summary>
        /// <value>The name of category.</value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets description of category .
        /// </summary>
        /// <value>The description of category.</value>
        public string CategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets deleted flag.
        /// </summary>
        /// <value>The deleted flag.</value>
        public bool DeletedFlag { get; set; }

        /// <summary>
        /// Gets and convert Product class to string.
        /// </summary>
        /// <return>System.String.</return>
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", CategoryId, CategoryName, CategoryDescription, DeletedFlag);
        }
    }

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
