using System;

namespace KMS.Next.CodeQuality.CSV.DTO
{
    /// <summary>
    /// Class Product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets product name.
        /// </summary>
        /// <value>The product name.</value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets product price.
        /// </summary>
        /// <value>The product price.</value>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets product description.
        /// </summary>
        /// <value>The product description.</value>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets product expired date.
        /// </summary>
        /// <value>The product expired date.</value>
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Gets or sets category identifier of product.
        /// </summary>
        /// <value>The category identifier of product.</value>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets deleted flag.
        /// </summary>
        /// <value>The deleted flag.</value>
        public bool DeletedFlag { get; set; }

        /// <summary>
        /// Gets and convert Product class to string.
        /// </summary>
        /// <return>System.String.</return>
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", ProductId, ProductName, Price, ProductDescription, ExpiredDate.ToShortDateString(), CategoryId, DeletedFlag);
        }
    }
}
