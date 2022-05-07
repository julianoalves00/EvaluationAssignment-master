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
        private Guid customerIdTest = Guid.NewGuid();

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
            PreLoadCustomers();
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
            CustomerItem result = CustomerController.Get(customerIdTest);

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

        #region Private methods
        private void PreLoadCustomers()
        {
            CustomerController.Create(new CustomerItem
            {
                Id = customerIdTest,
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
            });

            CustomerController.Create(new CustomerItem
            {
                Id = Guid.NewGuid(),
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
            });

            CustomerController.Create(new CustomerItem
            {
                Id = Guid.NewGuid(),
                Name = "Customer Y",
                FreeDaysPromotion = 200,
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
            });

            CustomerController.Create(new CustomerItem
            {
                Id = Guid.NewGuid(),
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
            });
        }    
        #endregion
    }
}
