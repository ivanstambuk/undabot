using System.Collections.Generic;
using System.Threading.Tasks;
using Undabot.Domain.Entities;
using Undabot.Domain.Responses.Product;

namespace Undabot.Domain.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int? maxprice, string size = null, string highlight = null);

        ProductFilter GetProductFilter();
    }
}