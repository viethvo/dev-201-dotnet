using KMS.Next.CodeQuality.CSV.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KMS.Next.CodeQuality.CSV
{
    /// <summary>
    /// Class CsvHelper.
    /// </summary>
    public class CsvHelper
    {
        /// <summary>
        /// Reads content from csv file.
        /// </summary>
        /// <param name = "path">Path of  csv file.</param>
        /// <return>List of T.</return>
        public List<T> ReadFromFile<T>(string path)
        {
            // Check path
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
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
            var listResult = lines.Result != null ? ConvertToList<T>(lines.Result) : null;

            return listResult ?? null;
        }

        /// <summary>
        /// Exports the product count of category to csv file.
        /// </summary>
        /// <param name = "categoryList">The category list.</param>
        /// <param name = "productList">The product list.</param>
        /// <param name = "path">The path of csv file.</param>
        /// <return>Task.</return>
        public async Task ExportCategoryCount(List<Category> categoryList, List<Product> productList, string path)
        {
            // Check path
            if (!CheckValidPath(path))
            {
                return;
            }

            // Check file
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Check list
            if (categoryList == null
                || productList == null
                || categoryList.Count == 0
                || productList.Count == 0)
            {
                return;
            }

            // Get map list and content
            var mapList = MapAndCount(categoryList, productList);
            var content = mapList != null ? ConvertListCount(mapList) : null;

            // Write file
            if (content != null)
            {
                await WriteCsvFile(path, content);
            }
        }

        /// <summary>
        /// Exports the list of product expired in next month.
        /// </summary>
        /// <param name = "categoryList">The category list.</param>
        /// <param name = "productList">The product list.</param>
        /// <param name = "path">The path of csv file.</param>
        /// <return>Task.</return>
        public async Task ExportProductExpiredNextMonth(List<Category> categoryList, List<Product> productList, string path)
        {
            // Check path
            if (!CheckValidPath(path))
            {
                return;
            }

            // Check file
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Check list
            if (categoryList == null
                || productList == null
                || categoryList.Count == 0
                || productList.Count == 0)
            {
                return;
            }

            var listExpired = GetProductExpiredNextMonth(categoryList, productList);
            var result = listExpired != null ? ConvertProductListExpired(listExpired) : null;

            // Write file
            if (result != null)
            {
                await WriteCsvFile(path, result);
            }
        }

        /// <summary>
        /// Writes content of csv file.
        /// </summary>
        /// <param name = "path">The path of csv file.</param>
        /// <param name = "content">The list string of csv content.</param>
        /// <return>Task.</return>
        private async Task WriteCsvFile(string path, List<string> content)
        {
            // Check file
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Write file
            var writer = File.OpenWrite(path);
            var streamWriter = new StreamWriter(writer);

            foreach (var line in content)
            {
                await streamWriter.WriteLineAsync(line);
            }

            // Close stream
            streamWriter.Close();
            writer.Close();
        }

        /// <summary>
        /// Gets category and its product count to list.
        /// </summary>
        /// <param name = "categoryList">The category list.</param>
        /// <param name = "productList">The product list.</param>
        /// <return>Dictionary.</return>
        private Dictionary<string, string> MapAndCount(List<Category> categoryList, List<Product> productList)
        {
            int total = productList.Count; // total products
            int totalCID = 0; // total products which have category ID

            // Create result map list
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("CategoryName", "ProductCount");

            foreach (var category in categoryList)
            {
                int count = productList.FindAll(p => p.CategoryId == category.CategoryId).Count;
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
            return result.Count > 0 ? result : null;
        }

        /// <summary>
        /// Gets product list expired next month.
        /// </summary>
        /// <param name = "categoryList">The category list.</param>
        /// <param name = "productList">The product list.</param>
        /// <return>List of ProductExpired.</return>
        private List<ProductExpired> GetProductExpiredNextMonth(List<Category> categoryList, List<Product> productList)
        {
            // Create result list
            List<ProductExpired> resultList = new List<ProductExpired>();

            foreach (var product in productList)
            {
                // Check if expired
                DateTime now = DateTime.Now;
                double gapDay = (product.ExpiredDate - now).TotalDays;

                if (gapDay > 31 || gapDay < 0)
                {
                    continue;
                }

                foreach (var category in categoryList)
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
                        resultList.Add(productExpired);
                        break; // break if match
                    }
                }
            }
            return resultList.Count > 0 ? resultList : null;
        }

        /// <summary>
        /// Converts list product expired to string.
        /// </summary>
        /// <param name = "listExpired">The list product expired.</param>
        /// <return>System.Text.StringBuilder.</return>
        private List<string> ConvertProductListExpired(List<ProductExpired> listExpired)
        {
            List<string> result = new List<string>();
            string header = string.Empty;
            var listProperties = typeof(ProductExpired).GetProperties();

            foreach (var property in listProperties)
            {
                header = string.Concat(header, ',', property.Name);
            }

            // Add header
            result.Add(header.TrimStart(','));

            // Convert to list string
            foreach (var expired in listExpired)
            {
                string temp = string.Format("{0},{1},{2},{3}", expired.ProductId, expired.ProductName, expired.CategoryName, expired.ExpiredDate.ToShortDateString());
                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// Converts list count map to string.
        /// </summary>
        /// <param name = "listMap">List map between category and product.</param>
        /// <return>List.</return>
        private List<string> ConvertListCount(Dictionary<string, string> listMap)
        {
            List<string> result = new List<string>();

            foreach (var map in listMap)
            {
                string temp = string.Format("{0},{1}", map.Key, map.Value);
                result.Add(temp);
            }
            return result.Count > 0 ? result : null;
        }

        /// <summary>
        /// Converts content of csv file to list.
        /// </summary>
        /// <param name = "lines">Content of csv file.</param>
        /// <return>List of T.</return>
        private List<T> ConvertToList<T>(string[] lines)
        {
            bool isHeader = true;
            var propListT = typeof(T).GetProperties();
            var listResult = new List<T>();

            foreach (string line in lines)
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
            if (listResult.Count == 0)
            {
                return null;
            }

            // Check if all values are null
            // In the case valid file but invalid type
            int count = listResult.FindAll(x => x == null).Count;
            return count != listResult.Count ? listResult : null;
        }

        /// <summary>
        /// Reads content of csv file.
        /// </summary>
        /// <param name = "path">Path of category csv file.</param>
        /// <return>List of T</return>
        private async Task<string[]> ReadAllLines(string path)
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
            return !string.IsNullOrEmpty(fileText) ? fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None) : null;
        }

        /// <summary>
        /// Checks valid path or not.
        /// </summary>
        /// <param name = "path">Path of category csv file.</param>
        /// <return>System.Boolean.</return>
        public static bool CheckValidPath(string path)
        {
            string extension = Path.GetExtension(path);

            bool isValid = path.IndexOfAny(Path.GetInvalidPathChars()) == -1;

            if (string.IsNullOrEmpty(path) || !isValid || !extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
