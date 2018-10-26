using KMS.Next.CodeQuality.CSV.DTO;
using System;
using System.Collections.Generic;
using System.IO;
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
            var mapList = MapAndCount(listCategory, listProduct);
            string content = ConvertListCountMapToString(mapList);

            // Write file
            await WriteCsvFile(path, content);
        }

        /// <summary>
        /// Exports the list of product expired in next month.
        /// </summary>
        /// <param name = "listCategory">List of category.</param>
        /// <param name = "listProduct">List of product.</param>
        /// <param name = "path">The path of csv file.</param>
        /// <return>Task.</return>
        public static async Task ExportFileExpiredNextMonth(List<Category> listCategory, List<Product> listProduct, string path)
        {
            var listExpired = GetProductExpiredNextMonth(listCategory, listProduct);
            string content = ConvertListExpiredToString(listExpired);

            // Write file
            await WriteCsvFile(path, content);
        }

        /// <summary>
        /// Writes content of csv file.
        /// </summary>
        /// <param name = "path">The path of csv file.</param>
        /// <param name = "content">The content of csv file.</param>
        /// <return>Task.</return>
        private static async Task WriteCsvFile(string path, string content)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Write file
            var writer = File.OpenWrite(path);
            var streamWriter = new StreamWriter(writer);
            await streamWriter.WriteAsync(content);

            // Close stream
            streamWriter.Close();
            writer.Close();
        }

        /// <summary>
        /// Gets list map and count between category and product.
        /// </summary>
        /// <param name = "listCategory">List of category.</param>
        /// <param name = "listProduct">List of product.</param>
        /// <return>Dictionary.</return>
        private static Dictionary<string, string> MapAndCount(List<Category> listCategory, List<Product> listProduct)
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
        /// Gets list of product expired next month.
        /// </summary>
        /// <param name = "listCategory">List of category.</param>
        /// <param name = "listProduct">List of product.</param>
        /// <return>List.</return>
        private static List<ProductExpired> GetProductExpiredNextMonth(List<Category> listCategory, List<Product> listProduct)
        {
            // Create result list
            List<ProductExpired> listResult = new List<ProductExpired>();

            foreach (var product in listProduct)
            {
                // Check if expired
                DateTime now = DateTime.Now;
                double gapDay = (product.ExpiredDate - now).TotalDays;

                if (gapDay > 31 || gapDay < 0)
                {
                    continue;
                }

                foreach (var category in listCategory)
                {
                    var productExpired = new ProductExpired
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ExpiredDate = product.ExpiredDate
                    };

                    // Check and map product and category
                    if (product.CategoryId == category.CategoryId)
                    {
                        productExpired.CategoryName = category.CategoryName;
                    }

                    if (product.CategoryId == null)
                    {
                        productExpired.CategoryName = "Others";
                    }

                    // Add to list
                    if (!string.IsNullOrEmpty(productExpired.CategoryName))
                    {
                        listResult.Add(productExpired);
                        break; // break if match
                    }
                }
            }
            return listResult;
        }

        private static string ConvertListExpiredToString(List<ProductExpired> listExpired)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var expired in listExpired)
            {
                string temp = string.Format("{0},{1},{2},{3}", expired.ProductId, expired.ProductName, expired.CategoryName, expired.ExpiredDate);
                builder.AppendLine(temp);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Converts list count map to string.
        /// </summary>
        /// <param name = "listMap">List map between category and product.</param>
        /// <return>System.String.</return>
        private static string ConvertListCountMapToString(Dictionary<string, string> listMap)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var map in listMap)
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
}
