using Microsoft.Extensions.Logging;
using QLabs.Customer.Application.Repositories;
using QLabs.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Customer.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private CustomerRepository _customerRepository;
        private ServiceItemRepository _serviceRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
            _customerRepository = CustomerRepository.Instance;
            _serviceRepository = ServiceItemRepository.Instance;
        }

        public List<CustomerItem> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public CustomerItem Get(Guid id)
        {
            CustomerItem customerItem = _customerRepository.Get(id);

            customerItem.Contracts?.ForEach(c => c.Service = _serviceRepository.Get(c.ServiceId));

            return customerItem;
        }

        public CustomerItem Create(CustomerItem customerRegister)
        {
            if(customerRegister.Id == Guid.Empty)
                customerRegister.Id = Guid.NewGuid();
            customerRegister.Contracts?.ForEach(c => c.CustumerId = customerRegister.Id);

            _customerRepository.Add(customerRegister);

            return customerRegister;
        }

        
    }
}
