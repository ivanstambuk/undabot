using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Undabot.Domain.Entities;
using Undabot.Domain.Logging;
using Undabot.Domain.Responses.Product;
using Undabot.Extensions;

namespace Undabot.Domain.Services
{
    /// <summary>
    /// Product service, scoped to the request
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<IProductService> _logger;

        private IEnumerable<Product> _allProducts;
        private IEnumerable<string> _allSizes;

        public ProductService(
            IProductRepository productRepository,
            ILogger<IProductService> logger
            )
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync(
            int? maxprice = null,
            string size = null,
            string highlight = null)
        {
            if (maxprice.HasValue && maxprice.Value <= 0)
            {
                _logger.LogWarning(Events.Get, Messages.InvalidMaxprice_price, maxprice);
                throw new ArgumentOutOfRangeException("maxprice filter is invalid");
            }

            if (_allProducts == null)
            {
                _allProducts = await _productRepository.GetAsync();

                _logger.LogInformation(Events.Get, Messages.ProductsFetched_number, _allProducts.Count());

                _allSizes = _allProducts.SelectMany(p => p.sizes).Distinct();
            }

            // Make a fresh copy to filter
            IEnumerable<Product> products = new List<Product>(_allProducts);

            if (!string.IsNullOrEmpty(size))
            {
                if (!_allSizes.Any(s => String.Equals(s, size, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentOutOfRangeException("Invalid size");
                }
            }

            if (maxprice.HasValue)
            {
                products = products.Where(p => p.price <= maxprice);
            }

            if (!string.IsNullOrEmpty(size))
            {
                size = size.Trim();
                products = products.Where(
                    p => p.sizes.Any(
                        s => String.Equals(s, size, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(highlight))
            {
                products = products.Select(p =>
                {
                    p.description = p.description.TagKeywords(highlight, "em");
                    return p;
                });
            }

            return products;
        }

        public ProductFilter GetProductFilter()
        {
            IEnumerable<string> getCommonWords(IEnumerable<Product> products, int skip = 5, int take = 10) =>
                                    products.SelectMany(p => p.description.SplitToWords())
                                        .GroupBy(w => w)
                                        .OrderByDescending(g => g.Count())
                                        .Select(g => g.Key)
                                        .Skip(skip)
                                        .Take(take);

            var productFilter = new ProductFilter()
            {
                maxPrice = _allProducts.Max(p => p.price),
                minPrice = _allProducts.Min(p => p.price),
                allSizes = _allSizes,
                commonWords = getCommonWords(_allProducts)
            };

            return productFilter;
        }
    }
}