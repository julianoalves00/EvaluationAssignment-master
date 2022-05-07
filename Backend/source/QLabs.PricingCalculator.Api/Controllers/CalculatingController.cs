using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLabs.Customer.Application.Services;
using QLabs.PricingCalculator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLabs.PricingCalculator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatingController
    {
        private readonly ILogger<CalculatingController> _logger;
        private IPricingServiceService _service;
        public CalculatingController(ILogger<CalculatingController> logger, IPricingServiceService service)
        {
            _logger = logger;
            _service = service;
        }

        [Route("CalculatePrice")]
        [HttpPost]
        public Decimal CalculatePrice(CalculatePrice calculatePrice)
        {
            _logger.LogInformation($"CalculatePrice({calculatePrice.CustomerId}, " +
                $"{calculatePrice.StartDate.ToString("yyyy-MM-dd")}, {calculatePrice.FinishDate.ToString("yyyy-MM-dd")})");

            return _service.CalculatingPrices(calculatePrice);
        }
    }
}
