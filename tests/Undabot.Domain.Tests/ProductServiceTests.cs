using Undabot.Domain.Entities;
using Undabot.Domain.Services;
using Xunit;
using Moq;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Undabot.Domain.Tests
{
    public class ProductServiceTests
    {
        private readonly IProductRepository _productRepository;
        private readonly Mock<ILogger<IProductService>> _logger;

        public ProductServiceTests()
        {
            _productRepository = new MemoryProductRepository();
            _logger = new Mock<ILogger<IProductService>>();
        }

        [Fact]
        public async Task getproducts_should_return_right_data()
        {
            var productService = new ProductService(_productRepository, _logger.Object);
            var products = await productService.GetProductsAsync();
            products.ShouldNotBeNull();
        }

        [Fact]
        public void getproducts_should_throw_exception_for_invalid_maxprice()
        {
            var productService = new ProductService(_productRepository, _logger.Object);
            productService.GetProductsAsync(-10).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void getproducts_should_throw_exception_for_invalid_size()
        {
            var productService = new ProductService(_productRepository, _logger.Object);
            productService.GetProductsAsync(11, "mega").ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task getproducts_should_filter_properly()
        {
            var productService = new ProductService(_productRepository, _logger.Object);
            var products = await productService.GetProductsAsync(11, "large");
            products.ShouldNotBeNull();
            products.ShouldNotBeEmpty();
            products.Count().ShouldBe(3);
        }

        [Fact]
        public async Task getfilter_should_return_right_data()
        {
            var productService = new ProductService(_productRepository, _logger.Object);
            await productService.GetProductsAsync();
            var productFilter = productService.GetProductFilter();

            productFilter.ShouldNotBeNull();
            productFilter.minPrice.ShouldBe(10);
            productFilter.maxPrice.ShouldBe(25);

            productFilter.allSizes.ShouldNotBeNull();
            productFilter.allSizes.ShouldNotBeEmpty();
            productFilter.allSizes.Count().ShouldBe(3);
            productFilter.allSizes.ShouldContain("small");
            productFilter.allSizes.ShouldContain("medium");
            productFilter.allSizes.ShouldContain("large");

            productFilter.commonWords.ShouldNotBeNull();
            productFilter.commonWords.ShouldNotBeEmpty();
            productFilter.commonWords.Count().ShouldBe(10);
            productFilter.commonWords.ShouldContain("hat");
            productFilter.commonWords.ShouldContain("shirt");
            productFilter.commonWords.ShouldContain("trouser");
            productFilter.commonWords.ShouldContain("green");
            productFilter.commonWords.ShouldContain("blue");
            productFilter.commonWords.ShouldContain("red");
            productFilter.commonWords.ShouldContain("belt");
            productFilter.commonWords.ShouldContain("bag");
            productFilter.commonWords.ShouldContain("shoe");
            productFilter.commonWords.ShouldContain("tie");
        }
    }
}
