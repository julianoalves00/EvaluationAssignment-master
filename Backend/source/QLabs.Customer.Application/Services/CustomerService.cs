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
        private CustomerRepository _repository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
            _repository = CustomerRepository.Instance;
        }

        public List<CustomerItem> GetAll()
        {
            return _repository.GetAll();
        }

        public CustomerItem Get(Guid id)
        {
            CustomerItem entity = _repository.Get(id);

            entity.Contracts?.ForEach(c => c.Service = ServiceItemRepository.Instance.Get(c.ServiceId) ?? null);

            return entity;
        }

        public CustomerItem Create(CustomerItem entity)
        {
            entity.Contracts?.ForEach(c => c.CustumerId = entity.Id);

            _repository.Add(entity);

            return entity;
        }
    }
}
