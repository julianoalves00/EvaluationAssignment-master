using Microsoft.Extensions.Logging;
using QLabs.Common;
using QLabs.Customer.Application.Services;
using QLabs.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Customer.Application.Repositories
{
    public class CustomerRepository
    {
        private static CustomerRepository _instance;
        private static Dictionary<Guid, CustomerItem> customers = new Dictionary<Guid, CustomerItem>();

        private CustomerRepository()
        {
        }

        public static CustomerRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CustomerRepository();
                    PreLoad();
                }

                return _instance;
            }
        }

        public void Add(CustomerItem entity)
        {
            customers.Add(entity.Id, entity);
        }

        public void Delete(Guid id)
        {
            customers.Remove(id);
        }

        public CustomerItem Get(Guid id)
        {
            return customers[id];
        }

        public List<CustomerItem> GetAll()
        {
            return customers.Values.ToList();
        }

        public void Update(CustomerItem entity)
        {
            customers[entity.Id] = entity;
        }

        private static void PreLoad()
        {
            // _Customer A_ only pays € 0,15 per working day for _Service A_ but pays € 0, 25 per working day for _Service B_).
            CustomerItem customerA = new CustomerItem
            {
                Id = Constants.Customer_A_Id,
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

            CustomerRepository.Instance.Add(customerA);

            // _Customer X_ started using _Service A_ and _Service C_ 2019 - 09 - 20.
            // _Customer X_ also had an discount of 20 % between 2019 - 09 - 22 and 2019 - 09 - 24 for _Service C_.
            CustomerItem customerX = new CustomerItem
            {
                Id = Constants.Customer_X_Id,
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

            CustomerRepository.Instance.Add(customerX);

            // _Customer Y_ started using _Service B_ and _Service C_ 2018 - 01 - 01.
            // _Customer Y_ had 200 free days and a discount of 30 % for the rest of the time.
            CustomerItem customerY = new CustomerItem
            {
                Id = Constants.Customer_Y_Id,
                Name = "Customer Y",
                FreeDaysPromotion = 200,
                Contracts = new List<ContractService>()
                {
                    new ContractService
                    {
                        ServiceId = Constants.Service_B_Id,
                        StartDate = new DateTime(2018, 1, 1),
                        DiscountPeriod = new DiscountPeriod
                        {
                            Percentage = 30.0M
                        }
                    },
                    new ContractService
                    {
                        ServiceId = Constants.Service_C_Id,
                        StartDate = new DateTime(2018, 1, 1),
                        DiscountPeriod = new DiscountPeriod
                        {
                            Percentage = 30.0M
                        }
                    }
                }
            };

            CustomerRepository.Instance.Add(customerY);
        }
    }
}
