using System;

namespace KMS.Next.CodeQuality.CSV.DTO
{
    /// <summary>
    /// Class ProductExpired.
    /// </summary>
    public class ProductExpired
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
        /// Gets or sets category name.
        /// </summary>
        /// <value>The category name.</value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets product expired date.
        /// </summary>
        /// <value>The product expired date.</value>
        public DateTime ExpiredDate { get; set; }
    }
}
