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
            //var a = CSVHeper.ReadAllLines("Data\\category.csv");

            //List<Category> listCategory = CSVHeper.ReadFromFile<Category>("Data\\category.csv");

            //listCategory.PrintAll();

            List<Product> listProduct = CsvHelper.ReadFromFile<Product>("Data\\product.csv");

            //listProduct.PrintAll();

            listProduct.WriteToFile("Data\\product1.csv").Wait();


            Console.ReadKey();
        }
    }
}
