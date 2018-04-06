using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            // Read product 
            var lines = File.ReadLines("Data//product.csv");
            List<Product> products = new List<Product>();
            var isHeader = false;
            var lists = typeof(Product).GetProperties();
            string[] line01 = null;
            foreach (var line in lines)
            {
                // Todo: Need to fix. The below line of code will cause exception 
                // whenever a field value contains comma (,) character
                string[] arr = line.Split(',');
                if(!isHeader)
                {
                    foreach (var item in lists)
                    {
                        var tmp = arr.Any(x => x == item.Name);
                        if(!tmp)
                        {
                            Console.WriteLine("Property not exists " + item.Name);
                        }
                    }
                    line01 = arr;
                    isHeader = true;
                    continue;
                }
                var p = new Product();
                foreach (var prop in lists)
                {
                    var index = Array.FindIndex(line01, x => x == prop.Name);
                    if(arr[index] != string.Empty)
                        prop.SetValue(p,Convert.ChangeType(arr[index],prop.PropertyType));
                }
                products.Add(p);
            }
            Console.WriteLine($"Name    Price   Description   Expired Date");
            foreach (var item in products)
            {
                Console.WriteLine($"{item.ProductName}  {item.Price}    {item.ProductDescription}    {item.ExpiredDate}");
            }
            // Read category

        }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string ProductDescription { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int CategoryId { get; set; }
        public bool DeletedFlag { get; set; }
    }
}
