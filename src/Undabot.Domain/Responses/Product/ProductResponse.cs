using System.Collections.Generic;

namespace Undabot.Domain.Responses.Product
{
    /// <summary>
    /// Product response
    /// </summary>
    public class ProductResponse
    {
        /// <summary>
        /// Resulting products
        /// </summary>
        /// <value></value>
        public IEnumerable<Undabot.Domain.Entities.Product> Products { get; set; }
        /// <summary>
        /// Product filter object
        /// </summary>
        /// <value></value>
        public ProductFilter ProductFilter { get; set; }
    }
}