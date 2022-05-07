using Microsoft.Extensions.Logging;
using Moq;
using QLabs.Common;
using QLabs.Customer.Api.Controllers;
using QLabs.Customer.Application.Services;
using QLabs.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QLabs.Test
{
    public class CustomerTest
    {
        private CustomerController CustomerController
        {
            get
            {
                var mockCustomerService = new Mock<ILogger<CustomerService>>();
                ILogger<CustomerService> loggerCustomerService = mockCustomerService.Object;

                ICustomerService service = new CustomerService(loggerCustomerService);

                var mockCustomerController = new Mock<ILogger<CustomerController>>();
                ILogger<CustomerController> loggerCustomerController = mockCustomerController.Object;

                return new CustomerController(loggerCustomerController, service);
            }
        }

        public CustomerTest()
        {

        }
        [Fact]
        public void ListAll()
        {
            //Arrange

            //Act
            IEnumerable<CustomerItem> result = CustomerController.Get();

            //Assert
            Assert.True(result != null && result.Count() > 0);
        }

        [Fact]
        public void GetCustomer()
        {
            //Arrange
            
            //Act
            CustomerItem result = CustomerController.Get(Constants.Customer_X_Id);

            //Assert
            Assert.True(result != null);
        }

        [Fact]
        public void CreateCustomer()
        {
            //Arrange
            CustomerItem customer = new CustomerItem
            {
                Name = "Customer Test",
                FreeDaysPromotion = 15,
                Contracts = new List<ContractService>()
                {
                    new ContractService
                    {
                        ServiceId = Constants.Service_A_Id,
                        WorkDayPrice = 0.15M
                    },
                    new ContractService
                    {
                        ServiceId = Constants.Service_B_Id
                    }
                    ,
                    new ContractService
                    {
                        ServiceId = Constants.Service_B_Id
                    }
                }
            };

            //Act
            CustomerItem result = CustomerController.Create(customer);

            CustomerItem returned = null;

            if(result != null && result.Id != Guid.Empty)
                returned = CustomerController.Get(result.Id);

            //Assert
            Assert.True(returned != null);
        }
    }
}
