using KMS.Next.CodeQuality.CSV.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KMS.Next.CodeQuality.Tests.CSV.DTO
{
    [TestClass]
    public class ListExtensionTests
    {
        private string categoryMockPath;
        private string productMockPath;

        [TestInitialize]
        public void Init()
        {
            string currentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string format = "{0}\\{1}\\{2}.csv";
            categoryMockPath = string.Format(format, currentPath, "Export", "list_category");
            productMockPath = string.Format(format, currentPath, "Export", "list_product");
            if (File.Exists(categoryMockPath))
            {
                File.Delete(categoryMockPath);
            }

            if (File.Exists(productMockPath))
            {
                File.Delete(productMockPath);
            }
        }

        [TestMethod]
        public async Task ListExtension_WriteToFile_WithEmptyListCategory_ShouldDoNothing()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            await list.WriteToFile(categoryMockPath);
            bool isFileExist = File.Exists(categoryMockPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task ListExtension_WriteToFile_WithEmptyListProduct_ShouldDoNothing()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            await list.WriteToFile(productMockPath);
            bool isFileExist = File.Exists(productMockPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task ListExtension_WriteToFile_WithValidProductList_ShouldExportCorrectly()
        {
            // arrange
            List<Product> list = new List<Product>();
            var product1 = new Product
            {
                ProductId = 1,
                ProductName = "Sugar",
                ProductDescription = "No description",
                CategoryId = 0,
                ExpiredDate = DateTime.Now,
                DeletedFlag = false
            };

            var product2 = new Product
            {
                ProductId = 2,
                ProductName = "Food",
                ProductDescription = "No description",
                CategoryId = 1,
                ExpiredDate = DateTime.Now,
                DeletedFlag = false
            };

            list.Add(product1);
            list.Add(product2);

            // act
            await list.WriteToFile(productMockPath);
            bool isFileExist = File.Exists(productMockPath);

            // assert
            Assert.AreEqual(isFileExist, true);
        }

        [TestMethod]
        public async Task ListExtension_WriteToFile_WithValidCategoryList_ShouldExportCorrectly()
        {
            // arrange
            List<Category> list = new List<Category>();
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "Food",
                CategoryDescription = "No",
                DeletedFlag = false
            };

            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "Water",
                CategoryDescription = "No",
                DeletedFlag = false
            };

            list.Add(category1);
            list.Add(category2);

            // act
            await list.WriteToFile(categoryMockPath);
            bool isFileExist = File.Exists(categoryMockPath);

            // assert
            Assert.AreEqual(isFileExist, true);
        }
    }
}
