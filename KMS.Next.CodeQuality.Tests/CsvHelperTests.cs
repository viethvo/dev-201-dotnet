using KMS.Next.CodeQuality.CSV;
using KMS.Next.CodeQuality.CSV.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KMS.Next.CodeQuality.Tests
{
    [TestClass]
    public class CsvHelperTests
    {
        private string productPath, categoryPath, invalidPath;
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
            categoryPath = currentPath + "\\Data\\category.csv";
            productPath = currentPath + "\\Data\\product.csv";
            invalidFilePath = currentPath + "\\Data\\invalid.csv";
            categoryCountPath = currentPath + "\\Data\\category_count.csv";
            productExpiredPath = currentPath + "\\Data\\product_expired.csv";
            invalidPath = "* invalid )(";
            validMockPath = productPath;
            csvHelper = new CsvHelper();

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
        public void CsvHelper_ReadFromCategoryFile_WithEmptyPath_ShouldReturnNull()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(string.Empty);

            // assert
            Assert.IsNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithInvalidPath_ShouldReturnNull()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(invalidPath);

            // assert
            Assert.IsNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithEmptyPath_ShouldReturnNull()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(string.Empty);

            // assert
            Assert.IsNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithInvalidPath_ShouldReturnNull()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(invalidPath);

            // assert
            Assert.IsNull(list);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "This type is not supported!")]
        public void CsvHelper_ReadFromCsvFile_WithInvalidClassAndValidPath_ShouldReturnNull()
        {
            // arrange
            List<int> list = new List<int>();

            // act
            list = csvHelper.ReadFromFile<int>(validMockPath);

            // assert
            // catch exception
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithInvalidContent_ShouldReturnNull()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(invalidFilePath);

            // assert
            Assert.IsNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithInvalidContent_ShouldReturnNull()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(invalidFilePath);

            // assert
            Assert.IsNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithValidContent_ShouldReturnCorrectly()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(categoryPath);

            // assert
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithValidContent_ShouldReturnCorrectly()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(productPath);

            // assert
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromCategoryFile_WithValidContentButProductType_ShouldReturnNull()
        {
            // arrange
            List<Product> list = new List<Product>();

            // act
            list = csvHelper.ReadFromFile<Product>(categoryPath);

            // assert
            Assert.IsNull(list);
        }

        [TestMethod]
        public void CsvHelper_ReadFromProductFile_WithValidContentButCategoryType_ShouldReturnNull()
        {
            // arrange
            List<Category> list = new List<Category>();

            // act
            list = csvHelper.ReadFromFile<Category>(productPath);

            // assert
            Assert.IsNull(list);
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
            bool isFileExist = File.Exists(categoryCountPath);

            // assert
            Assert.AreEqual(isFileExist, true);
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
            bool isValidPath = CsvHelper.CheckValidPath(productPath);

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
