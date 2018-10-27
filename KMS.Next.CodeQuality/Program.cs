using KMS.Next.CodeQuality.CSV;
using KMS.Next.CodeQuality.CSV.DTO;
using System;
using System.Collections.Generic;

namespace KMS.Next.CodeQuality
{
    class Program
    {
        static void Main(string[] args)
        {
            /* USING CSV HELPER*/
            CsvHelper csvHelper = new CsvHelper();
            List<Category> listCategory = csvHelper.ReadFromFile<Category>("Data\\category.csv");
            listCategory.PrintAll();

            List<Product> listProduct = csvHelper.ReadFromFile<Product>("Data\\product.csv");
            listProduct.PrintAll();

            /* EXPORT FILES */
            csvHelper.ExportCategoryCount(listCategory, listProduct, "Data\\count_product_per_category.csv").Wait();
            csvHelper.ExportProductExpiredNextMonth(listCategory, listProduct, "Data\\product_expire_next_month.csv").Wait();

            listProduct.WriteToFile("Data\\product_export.csv").Wait();
            listCategory.WriteToFile("Data\\category_export.csv").Wait();

            Console.WriteLine("Successfully!");
            Console.ReadKey();
        }
    }
}
