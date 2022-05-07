using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using QLabs.Common;
using QLabs.Common.Domain;
using QLabs.Customer.Application.Services;
using QLabs.PricingCalculator.Api.Controllers;
using QLabs.PricingCalculator.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        /// <summary>
        /// _Customer X_ started using _Service A_ and _Service C_ 2019-09-20. 
        /// _Customer X_ also had an discount of 20% between 2019-09-22 and 2019-09-24 for _Service C_. 
        /// What is the total price for _Customer X_ up until 2019-10-01?
        /// </summary>
        [Fact]
        public async void TestCase1()
        {
            //Arrange
            decimal result = -1;
            Guid newGuid = Guid.NewGuid();

            CustomerItem customer = new CustomerItem
            {
                Id = newGuid,
                Name = "Customer X",
                Contracts = new List<ContractService>()
                {
                    new ContractService
                    {
                        ServiceId = Constants.Service_A_Id,
                        StartDate = new DateTime(2019, 9, 20)
                    },
                    new ContractService
                    {
                        ServiceId = Constants.Service_C_Id,
                        StartDate = new DateTime(2019, 9, 20),
                        DiscountPeriod = new DiscountPeriod
                        {
                            StartDate = new DateTime(2019, 9, 22),
                            FinishDate = new DateTime(2019, 9, 24),
                            Percentage = 20.0M
                        }
                    }
                }
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.GetSection("CustumerApiUrl").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"customer", httpContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not create consumer!");
            }

            //Act
            CalculatePrice calculatePrice = new CalculatePrice
            {
                CustomerId = newGuid,
                StartDate = new DateTime(2019, 9, 20),
                FinishDate = new DateTime(2019, 10, 01)
            };

            result = CalculatingControl.CalculatePrice(calculatePrice);

            //Assert
            Assert.True(result > 0);
        }

        /// <summary>
        /// _Customer Y_ started using _Service B_ and _Service C_ 2018-01-01. 
        /// _Customer Y_ had 200 free days and a discount of 30% for the rest of the time. 
        /// What is the total price for _Customer Y_ up until 2019-10-01?
        /// </summary>
        [Fact]
        public async void TestCase2()
        {
            //Arrange
            decimal result = -1;
            Guid newGuid = Guid.NewGuid();

            CustomerItem customer = new CustomerItem
            {
                Id = newGuid,
                Name = "Customer Y",
                FreeDaysPromotion = 200,
                Contracts = new List<ContractService>()
                {
                    new ContractService
                    {
                        ServiceId = Constants.Service_B_Id,
                        //StartDate = new DateTime(2018, 1, 1),
                        DiscountPeriod = new DiscountPeriod
                        {
                            Percentage = 30.0M
                        }
                    },
                    new ContractService
                    {
                        ServiceId = Constants.Service_C_Id,
                        //StartDate = new DateTime(2018, 1, 1),
                        DiscountPeriod = new DiscountPeriod
                        {
                            Percentage = 30.0M
                        }
                    }
                }
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.GetSection("CustumerApiUrl").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"customer", httpContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not create consumer!");
            }

            //Act
            CalculatePrice calculatePrice = new CalculatePrice
            {
                CustomerId = newGuid,
                StartDate = new DateTime(2018, 1, 1),
                FinishDate = new DateTime(2019, 10, 01)
            };

            result = CalculatingControl.CalculatePrice(calculatePrice);

            //Assert
            Assert.True(result > 0);
        }

        [Fact]
        public async void StartDateDiferentForOneService()
        {
            //Arrange
            decimal result = -1;
            Guid newGuid = Guid.NewGuid();

            CustomerItem customer = new CustomerItem
            {
                Id = newGuid,
                Name = "Customer Diff Server Price",
                Contracts = new List<ContractService>()
                {
                    new ContractService
                    {
                        ServiceId = Constants.Service_B_Id,
                        StartDate = new DateTime(2018, 6, 1),
                        DiscountPeriod = new DiscountPeriod
                        {
                            Percentage = 30.0M
                        }
                    },
                    new ContractService
                    {
                        ServiceId = Constants.Service_C_Id,
                        DiscountPeriod = new DiscountPeriod
                        {
                            Percentage = 30.0M
                        }
                    }
                }
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.GetSection("CustumerApiUrl").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"customer", httpContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not create consumer!");
            }

            //Act
            CalculatePrice calculatePrice = new CalculatePrice
            {
                CustomerId = newGuid,
                StartDate = new DateTime(2018, 1, 1),
                FinishDate = new DateTime(2019, 10, 01)
            };

            result = CalculatingControl.CalculatePrice(calculatePrice);

            //Assert
            Assert.True(result > 0);
        }

        /// <summary>
        /// _Customer A_ only pays € 0,15 per working day for _Service A_ but pays € 0,25 per working day for _Service B_
        /// </summary>
        [Fact]
        public async void DiferentPricesForServices()
        {
            //Arrange
            decimal result = -1;
            Guid newGuid = Guid.NewGuid();

            CustomerItem customer = new CustomerItem
            {
                Id = newGuid,
                Name = "Customer A",
                Contracts = new List<ContractService>()
                {
                    new ContractService
                    {
                        ServiceId = Constants.Service_A_Id,
                        WorkDayPrice = 0.15M
                    },
                    new ContractService
                    {
                        ServiceId = Constants.Service_B_Id,
                        WorkDayPrice = 0.25M
                    }
                }
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.GetSection("CustumerApiUrl").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"customer", httpContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not create consumer!");
            }

            //Act
            CalculatePrice calculatePrice = new CalculatePrice
            {
                CustomerId = newGuid,
                StartDate = new DateTime(2018, 1, 1),
                FinishDate = new DateTime(2019, 10, 01)
            };

            result = CalculatingControl.CalculatePrice(calculatePrice);

            //Assert
            Assert.True(result > 0);
        }
    }
}
