using KMS.Next.CodeQuality.CSV.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KMS.Next.CodeQuality.Tests.CSV.DTO
{
    [TestClass]
    public class CategoryTests
    {
        [TestMethod]
        public void Category_ToString_WithValidValues_ShouldReturnCorrectly()
        {
            // arrange
            int id = 1;
            string name = "Food";
            string description = "Food";
            bool delete = false;
            string compare = string.Format("{0},{1},{2},{3}", id, name, description, delete);

            // assert
            Category category = new Category
            {
                CategoryId = id,
                CategoryName = name,
                CategoryDescription = description,
                DeletedFlag = delete
            };

            // act
            Assert.AreEqual(category.ToString(), compare);
        }
    }
}
