using System.Collections.Generic;
using System.Threading.Tasks;

namespace Undabot.Domain.Entities
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAsync();
    }
}