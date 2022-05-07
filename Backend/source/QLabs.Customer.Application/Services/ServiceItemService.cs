using Microsoft.Extensions.Logging;
using QLabs.Customer.Application.Repositories;
using QLabs.Customer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Customer.Application.Services
{
    public class ServiceItemService : IServiceItemService
    {
        private ServiceItemRepository _repository;
        private readonly ILogger<ServiceItemService> _logger;

        public ServiceItemService(ILogger<ServiceItemService> logger)
        {
            _logger = logger;
            _repository = ServiceItemRepository.Instance;
        }

        public ServiceItem Create(ServiceItem customerRegister)
        {
            throw new NotImplementedException();
        }

        public ServiceItem Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<ServiceItem> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
