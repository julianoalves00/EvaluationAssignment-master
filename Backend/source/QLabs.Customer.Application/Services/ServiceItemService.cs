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
    public class ServiceItemService : IServiceItemService
    {
        private ServiceItemRepository _repository;
        private readonly ILogger<ServiceItemService> _logger;

        public ServiceItemService(ILogger<ServiceItemService> logger)
        {
            _logger = logger;
            _repository = ServiceItemRepository.Instance;
        }

        public List<ServiceItem> GetAll()
        {
            return _repository.GetAll();
        }
        public ServiceItem Get(Guid id)
        {
            ServiceItem entity = _repository.Get(id);

            return entity;
        }

        public ServiceItem Create(ServiceItem entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            

            _repository.Add(entity);

            return entity;
        }

        

        
    }
}
