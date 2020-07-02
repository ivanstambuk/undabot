using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Undabot.Domain.Responses.Product;
using Undabot.Domain.Services;

namespace Undabot.API.Controllers
{
    /// <summary>
    /// Products
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;

        public ProductController(
            ILogger<ProductController> logger,
            IConfiguration configuration,
            IProductService productService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _productService = productService;
        }

        /// <summary>
        /// Get a filtered list of products
        /// </summary>
        /// <param name="maxprice">Maximum product price filter</param>
        /// <param name="size">Product size filter</param>
        /// <param name="highlight">Comma-separated list of words to highlight</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponse>> Get(
            [FromQuery] int? maxprice = null,
            [FromQuery] string size = null,
            [FromQuery] string highlight = null)
        {
            var products = await _productService.GetProductsAsync(maxprice, size, highlight);
            var productFilter = _productService.GetProductFilter();

            var response = new ProductResponse()
            {
                Products = products,
                ProductFilter = productFilter
            };

            return Ok(response);
        }
    }
}