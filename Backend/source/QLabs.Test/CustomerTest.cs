using Moq;
using QLabs.Customer.Api.Controllers;
using QLabs.Customer.Application.Services;
using QLabs.Customer.Domain;
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
            ICustomerService service = new CustomerService();

            var controller = new CustomerController(service);

            //Act
            IEnumerable<CustomerItem> result = controller.Get();

            //Assert
            Assert.True(result != null && result.Count() > 0);
        }
    }
}
