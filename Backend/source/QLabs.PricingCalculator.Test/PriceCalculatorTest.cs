using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using QLabs.Common;
using QLabs.Customer.Application.Services;
using QLabs.PricingCalculator.Api.Controllers;
using QLabs.PricingCalculator.Domain;
using System;
using Xunit;

namespace QLabs.PricingCalculator.Test
{
    public class PriceCalculatorTest
    {
        [Fact]
        public void Calculate_X()
        {
            //Arrange
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables().Build();

            var mockPricingService = new Mock<ILogger<PricingServiceService>>();
            ILogger<PricingServiceService> loggerPricingService = mockPricingService.Object;

            IPricingServiceService service = new PricingServiceService(loggerPricingService, config);

            var mockCalculating = new Mock<ILogger<CalculatingController>>();
            ILogger<CalculatingController> loggerCalculating = mockCalculating.Object;

            var controller = new CalculatingController(loggerCalculating, service);

            //Act
            CalculatePrice calculatePrice = new CalculatePrice
            {
                CustomerId = Constants.Customer_X_Id,
                StartDate = new DateTime(2019, 9, 20),
                FinishDate = new DateTime(2019, 10, 01)
            };

            decimal result = controller.CalculatePrice(calculatePrice);

            //Assert
            Assert.True(result > 0);
        }
    }
}
