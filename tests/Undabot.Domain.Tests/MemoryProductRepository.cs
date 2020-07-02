using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Undabot.Domain.Entities;

namespace Undabot.Domain.Tests
{
    public class MemoryProductRepository : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetAsync()
        {
            var json = await File.ReadAllTextAsync(@"Data\products.json");

            JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            JsonElement productsElement = root.GetProperty("products");

            var allProducts = JsonSerializer.Deserialize<IEnumerable<Product>>(productsElement.GetRawText());
            return allProducts;
        }
    }
}