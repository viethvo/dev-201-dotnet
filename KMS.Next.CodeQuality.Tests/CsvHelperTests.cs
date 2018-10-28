using KMS.Next.CodeQuality.CSV;
using KMS.Next.CodeQuality.CSV.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KMS.Next.CodeQuality.Tests
{
    [TestClass]
    public class CsvHelperTests
    {
        private string productPath, productMockPath, categoryPath, categoryMockPath, invalidPath;
        private string invalidFilePath, validMockPath, categoryCountPath, productExpiredPath;
        private CsvHelper csvHelper;
        private Product product1;
        private Category category1;
        private List<Category> categoryFullList, categoryEmptyList;
        private List<Product> productFullList, productEmptyList;

        [TestInitialize]
        public void Init()
        {
            string currentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string format = "{0}\\{1}\\{2}.csv";
            categoryPath = string.Format(format, currentPath, "Export", "category");
            productPath = string.Format(format, currentPath, "Export", "product");
            categoryMockPath = string.Format(format, currentPath, "MockData", "category");
            productMockPath = string.Format(format, currentPath, "MockData", "product");
            invalidFilePath = string.Format(format, currentPath, "MockData", "invalid");
            categoryCountPath = string.Format(format, currentPath, "Export", "category_count");
            productExpiredPath = string.Format(format, currentPath, "Export", "product_expired");
            invalidPath = "* invalid )(";
            validMockPath = productPath;
            categoryEmptyList = Enumerable.Empty<Category>().ToList();
            productEmptyList = Enumerable.Empty<Product>().ToList();
            csvHelper = new CsvHelper();
            if (File.Exists(productPath))
            {
                File.Delete(productPath);
            }

            if (File.Exists(categoryPath))
            {
                File.Delete(categoryPath);
            }

            if (File.Exists(categoryCountPath))
            {
                File.Delete(categoryCountPath);
            }

            if (File.Exists(productExpiredPath))
            {
                File.Delete(productExpiredPath);
            }

            #region MockData

            product1 = new Product
            {
                ProductId = 1,
                ProductName = "Sugar",
                ProductDescription = "No description",
                CategoryId = 1,
                ExpiredDate = (DateTime.Now).AddMonths(1),
                DeletedFlag = false
            };

            category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "Food",
                CategoryDescription = "No",
                DeletedFlag = false
            };

            categoryFullList = new List<Category>();
            productFullList = new List<Product>();
            categoryEmptyList = new List<Category>();
            productEmptyList = new List<Product>();
            categoryFullList.Add(category1);
            productFullList.Add(product1);
            #endregion
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithEmptyPath_ShouldReturnEmptyList()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(string.Empty);

            // assert
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithInvalidPath_ShouldReturnEmptyList()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(invalidPath);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithEmptyPath_ShouldReturnEmptyList()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(string.Empty);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithInvalidPath_ShouldReturnEmptyList()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(invalidPath);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "This type is not supported!")]
        public void CsvHelper_ReadFromCsvFile_WithInvalidClassAndValidPath_ShouldReturnException()
        {
            // arrange
            List<int> list = new List<int>();

            // act
            list = csvHelper.ReadFromFile<int>(validMockPath);

            // assert
            // catch exception
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithInvalidContent_ShouldReturnEmptyList()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(invalidFilePath);

            // assert
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithInvalidContent_ShouldReturnEmptyList()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(invalidFilePath);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithValidContent_ShouldReturnCorrectly()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(categoryMockPath);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count > 0, true);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithValidContent_ShouldReturnCorrectly()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(productMockPath);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count > 0, true);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithValidContentButProductType_ShouldReturnEmptyArray()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(categoryMockPath);

            // assert
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithValidContentButCategoryType_ShouldReturnEmptyArray()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(productMockPath);

            // assert
            Assert.AreEqual(list.Count > 0, false);
        }

        [TestMethod]
        public async Task CsvHelper_ExportCategoryCount_WithInvalidListsAndValidPath_ShouldDoNothing()
        {
            // arrange
            var categoryList = categoryEmptyList;
            var productList = productEmptyList;

            // act
            await csvHelper.ExportCategoryCount(categoryList, productList, categoryCountPath);
            bool isFileExist = File.Exists(categoryCountPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task CsvHelper_ExportCategoryCount_WithInvalidListsAndInvalidPath_ShouldDoNothing()
        {
            // arrange
            var categoryList = categoryEmptyList;
            var productList = productEmptyList;

            // act
            await csvHelper.ExportCategoryCount(categoryList, productList, invalidPath);
            bool isFileExist = File.Exists(categoryCountPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task CsvHelper_ExportCategoryCount_WithValidListsAndValidPath_ShouldExportCorrectly()
        {
            // arrange
            var categoryList = categoryFullList;
            var productList = productFullList;

            // act
            await csvHelper.ExportCategoryCount(categoryList, productList, categoryCountPath);
            bool isFileExist = File.Exists(categoryCountPath);

            // assert
            Assert.AreEqual(isFileExist, true);
        }

        [TestMethod]
        public async Task CsvHelper_ExportCategoryCount_WithValidListsAndInvalidPath_ShouldDoNothing()
        {
            // arrange
            var categoryList = categoryFullList;
            var productList = productFullList;

            // act
            await csvHelper.ExportCategoryCount(categoryList, productList, invalidPath);
            bool isFileExist = File.Exists(invalidPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task CsvHelper_ExportProductExpiredNextMonth_WithInvalidListsAndValidPath_ShouldDoNothing()
        {
            // arrange
            var categoryList = categoryEmptyList;
            var productList = productEmptyList;

            // act
            await csvHelper.ExportProductExpiredNextMonth(categoryList, productList, productExpiredPath);
            bool isFileExist = File.Exists(productExpiredPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task CsvHelper_ExportProductExpiredNextMonth_WithInvalidListsAndInvalidPath_ShouldDoNothing()
        {
            // arrange
            var categoryList = categoryEmptyList;
            var productList = productEmptyList;

            // act
            await csvHelper.ExportProductExpiredNextMonth(categoryList, productList, invalidPath);
            bool isFileExist = File.Exists(productExpiredPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task CsvHelper_ExportProductExpiredNextMonth_WithValidListsAndInvalidPath_ShouldDoNothing()
        {
            // arrange
            var categoryList = categoryFullList;
            var productList = productFullList;

            // act
            await csvHelper.ExportProductExpiredNextMonth(categoryList, productList, invalidPath);
            bool isFileExist = File.Exists(productExpiredPath);

            // assert
            Assert.AreEqual(isFileExist, false);
        }

        [TestMethod]
        public async Task CsvHelper_ExportProductExpiredNextMonth_WithValidListsAndValidPath_ShouldExportCorrectly()
        {
            // arrange
            var categoryList = categoryFullList;
            var productList = productFullList;

            // act
            await csvHelper.ExportProductExpiredNextMonth(categoryList, productList, productExpiredPath);
            bool isFileExist = File.Exists(productExpiredPath);

            // assert
            Assert.AreEqual(isFileExist, true);
        }

        [TestMethod]
        public void CsvHelper_CheckValidPath_WithValidPath_ShouldReturnTrue()
        {
            // act
            bool isValidPath = CsvHelper.CheckValidPath(productMockPath);

            // assert
            Assert.AreEqual(isValidPath, true);
        }

        [TestMethod]
        public void CsvHelper_CheckValidPath_WithInvalidPath_ShouldReturnFalse()
        {
            // act
            bool isValidPath = CsvHelper.CheckValidPath(invalidPath);

            // assert
            Assert.AreEqual(isValidPath, false);
        }

        [TestMethod]
        public void CsvHelper_CheckValidPath_WithEmptyPath_ShouldReturnFalse()
        {
            // act
            bool isValidPath = CsvHelper.CheckValidPath(string.Empty);

            // assert
            Assert.AreEqual(isValidPath, false);
        }
    }
}
