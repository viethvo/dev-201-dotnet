using System;

namespace KMS.Next.CodeQuality.CSV.DTO
{

    /// <summary>
    /// Class ProductExpired.
    /// </summary>
    public class ProductExpired
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
        /// Gets or sets name of category .
        /// </summary>
        /// <value>The name of category.</value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets expired date of product.
        /// </summary>
        /// <value>The expired date of product.</value>
        public DateTime ExpiredDate { get; set; }
    }
}
