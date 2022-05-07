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
        [Fact]
        public void ListAll()
        {
            //Arrange
            var mockCustomerService = new Mock<ILogger<CustomerService>>();
            ILogger<CustomerService> loggerCustomerService = mockCustomerService.Object;

            ICustomerService service = new CustomerService(loggerCustomerService);

            var mockCustomerController = new Mock<ILogger<CustomerController>>();
            ILogger<CustomerController> loggerCustomerController = mockCustomerController.Object;

            var controller = new CustomerController(loggerCustomerController, service);

            //Act
            IEnumerable<CustomerItem> result = controller.Get();

            //Assert
            Assert.True(result != null && result.Count() > 0);
        }

        [Fact]
        public void GetCustomer()
        {
            //Arrange
            var mockCustomerService = new Mock<ILogger<CustomerService>>();
            ILogger<CustomerService> loggerCustomerService = mockCustomerService.Object;

            ICustomerService service = new CustomerService(loggerCustomerService);

            var mockCustomerController = new Mock<ILogger<CustomerController>>();
            ILogger<CustomerController> loggerCustomerController = mockCustomerController.Object;

            var controller = new CustomerController(loggerCustomerController, service);

            //Act
            CustomerItem result = controller.Get(Constants.Customer_X_Id);

            //Assert
            Assert.True(result != null);
        }


    }
}
