using System.Collections.Generic;

namespace Undabot.Domain.Entities
{
    /// <summary>
    /// Product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product title
        /// </summary>
        /// <value></value>
        public string title { get; set; }
        /// <summary>
        /// Product price, non-negative
        /// </summary>
        /// <value></value>
        public int price { get; set; }
        /// <summary>
        /// Sizes for the product
        /// </summary>
        /// <value></value>
        public IEnumerable<string> sizes { get; set; }
        /// <summary>
        /// Product description
        /// </summary>
        /// <value></value>
        public string description { get; set; }
    }
}
