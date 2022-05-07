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
        IConfiguration _config;

        private CalculatingController CalculatingControl
        {
            get
            {
                var mockPricingService = new Mock<ILogger<PricingServiceService>>();
                ILogger<PricingServiceService> loggerPricingService = mockPricingService.Object;

                IPricingServiceService service = new PricingServiceService(loggerPricingService, _config);

                var mockCalculating = new Mock<ILogger<CalculatingController>>();
                ILogger<CalculatingController> loggerCalculating = mockCalculating.Object;

                return new CalculatingController(loggerCalculating, service);
            }
        }

        public PriceCalculatorTest()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").AddEnvironmentVariables().Build();
        }

        [Fact]
        public void Calculate_X()
        {
            //Arrange
            CalculatePrice calculatePrice = new CalculatePrice
            {
                CustomerId = Constants.Customer_X_Id,
                StartDate = new DateTime(2019, 9, 20),
                FinishDate = new DateTime(2019, 10, 01)
            };

            //Act
            decimal result = CalculatingControl.CalculatePrice(calculatePrice);

            //Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void Calculate_Y()
        {
            //Arrange
            CalculatePrice calculatePrice = new CalculatePrice
            {
                CustomerId = Constants.Customer_Y_Id,
                StartDate = new DateTime(2018, 1, 1),
                FinishDate = new DateTime(2019, 10, 01)
            };

            //Act
            decimal result = CalculatingControl.CalculatePrice(calculatePrice);

            //Assert
            Assert.True(result > 0);
        }
    }
}
