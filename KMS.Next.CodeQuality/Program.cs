using KMS.Next.CodeQuality.CSV;
using System;
using System.Collections.Generic;
/// <summary>
/// This file contains a lot of code smells.
/// Todo: Refactor the code to make it clean
/// </summary>
namespace KMS.Next.CodeQuality
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Category> listCategory = CsvHelper.ReadFromFile<Category>("Data\\category.csv");
            //listCategory.PrintAll();

            List<Product> listProduct = CsvHelper.ReadFromFile<Product>("Data\\product.csv");
            //listProduct.PrintAll();

            CsvHelper.ExportFileMapBetween(listCategory, listProduct, "Data\\map.csv").Wait();

            //listProduct.WriteToFile("Data\\product1.csv").Wait();

            Console.ReadKey();
        }
    }
}
