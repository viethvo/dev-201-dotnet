using System;

namespace KMS.Next.CodeQuality.CSV.DTO
{
    /// <summary>
    /// Class Product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets identifier of product.
        /// </summary>
        /// <value>The identifier of product.</value>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets name of product.
        /// </summary>
        /// <value>The name of product.</value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets price of product.
        /// </summary>
        /// <value>The price of product.</value>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets description of product.
        /// </summary>
        /// <value>The description of product.</value>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets expired date of product.
        /// </summary>
        /// <value>The expired date of product.</value>
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
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", ProductId, ProductName, Price, ProductDescription, ExpiredDate, CategoryId, DeletedFlag);
        }
    }

}
