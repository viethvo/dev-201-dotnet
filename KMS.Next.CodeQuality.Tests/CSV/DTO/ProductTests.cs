using KMS.Next.CodeQuality.CSV.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KMS.Next.CodeQuality.Tests.CSV.DTO
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Product_ToString_WithValidValues_ShouldReturnCorrectly()
        {
            // arrange
            int id = 1;
            string name = "Sugar";
            double price = 30;
            string description = "type 1";
            DateTime expired = DateTime.Now;
            int cId = 2;
            bool delete = false;
            string compare = string.Format("{0},{1},{2},{3},{4},{5},{6}", id, name, price, description, expired.ToShortDateString(), cId, delete);

            // assert
            Product product = new Product
            {
                ProductId = id,
                ProductName = name,
                Price = price,
                ProductDescription = description,
                ExpiredDate = expired,
                CategoryId = cId,
                DeletedFlag = delete
            };

            // act
            Assert.AreEqual(product.ToString(), compare);
        }
    }
}
