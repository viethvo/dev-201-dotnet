namespace KMS.Next.CodeQuality.CSV.DTO
{
    /// <summary>
    /// Class Category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets category identifier.
        /// </summary>
        /// <value>The category identifier.</value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        /// <value>The category name.</value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets category description.
        /// </summary>
        /// <value>The category description.</value>
        public string CategoryDescription { get; set; }

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
            return string.Format("{0},{1},{2},{3}", CategoryId, CategoryName, CategoryDescription, DeletedFlag);
        }
    }
}
