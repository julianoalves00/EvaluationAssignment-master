using Microsoft.Extensions.Hosting;
using QLabs.Common;
using QLabs.Common.Domain;
using QLabs.Customer.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QLabs.Customer.Api.LoadData
{
    public class LoadDataService : IHostedService
    {
        IServiceItemService _serviceItem;
        ICustomerService _customerService;
        public LoadDataService(IServiceItemService serviceItem, ICustomerService customerService)
        {
            _serviceItem = serviceItem;
            _customerService = customerService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.FromResult(LoadServices());
            await Task.FromResult(LoadCustomers());
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }

        #region Private Methods

        private bool LoadServices()
        {
            // _Service A_ = € 0,2 / working day(monday - friday)
            ServiceItem serviceA = new ServiceItem
            {
                Id = Constants.Service_A_Id,
                Name = "Service A",
                WorkDayPrice = 0.2M,
                DaysOfTheWeek = Constants.WorkDays
            };

            _serviceItem.Create(serviceA);

            // _Service B_ = € 0,24 / working day(monday - friday)
            ServiceItem serviceB = new ServiceItem
            {
                Id = Constants.Service_B_Id,
                Name = "Service B",
                WorkDayPrice = 0.24M,
                DaysOfTheWeek = Constants.WorkDays
            };

            _serviceItem.Create(serviceB);

            // _Service C_ = € 0,4 / day(monday - sunday)
            ServiceItem serviceC = new ServiceItem
            {
                Id = Constants.Service_C_Id,
                Name = "Service C",
                WorkDayPrice = 0.4M,
                DaysOfTheWeek = Constants.AllDays
            };

            _serviceItem.Create(serviceC);

            return true;
        }

        private bool LoadCustomers()
        {
            CustomerItem customer = new CustomerItem
            {
                Id = Guid.NewGuid(),
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

            _customerService.Create(customer);

            customer = new CustomerItem
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
            };

            _customerService.Create(customer);

            customer = new CustomerItem
            {
                Id = Guid.NewGuid(),
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

            _customerService.Create(customer);

            customer = new CustomerItem
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
            };

            _customerService.Create(customer);

            return true;
        }

        #endregion
    }
}
