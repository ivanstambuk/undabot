using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Undabot.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Undabot.Domain.Services;
using Undabot.Domain.Logging;

namespace Undabot.Infrastructure
{
    /// <summary>
    /// In-memory repository
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private IEnumerable<Product> _products { get; set; }
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IProductRepository> _logger;

        public ProductRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<IProductRepository> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            var productEndpoint = _configuration["ProductsEndPoint"];
            _logger.LogInformation(Events.Get, Messages.ProductEndpoint_url, productEndpoint);

            var json = await _httpClient.GetStringAsync(productEndpoint);
            _logger.LogInformation(Events.Get, Messages.ProductEndpointResponse_text, json);

            JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            JsonElement productsElement = root.GetProperty("products");

            _products = JsonSerializer.Deserialize<IEnumerable<Product>>(productsElement.GetRawText());

            return _products;
        }
    }
}
