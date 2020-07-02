using System.Collections.Generic;

namespace Undabot.Domain.Responses.Product
{
    public class ProductFilter
    {
        /// <summary>
        /// The minimum price of all products
        /// </summary>
        /// <value></value>
        public int minPrice { get; set; }
        /// <summary>
        /// The maximum price of all products
        /// </summary>
        /// <value></value>
        public int maxPrice { get; set; }
        /// <summary>
        /// All sizes of all products
        /// </summary>
        /// <value></value>
        public IEnumerable<string> allSizes { get; set; }
        /// <summary>
        /// Most common words in the product descriptions, excluding the most common five
        /// </summary>
        /// <value></value>
        public IEnumerable<string> commonWords { get; set; }
    }
}