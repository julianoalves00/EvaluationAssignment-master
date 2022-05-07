using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLabs.Customer.Application.Services;
using QLabs.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLabs.Customer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceItemController : ControllerBase
    {
        private IServiceItemService _service;
        private readonly ILogger<ServiceItemController> _logger;

        public ServiceItemController(ILogger<ServiceItemController> logger, IServiceItemService service)
        {
            _logger = logger; 
            _service = service;
        }

        [HttpGet]
        public IEnumerable<ServiceItem> Get()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ServiceItem Get(Guid id)
        {
            return _service.Get(id);
        }

        [HttpPost]
        public ServiceItem Create(ServiceItem customerRegister)
        {
            return _service.Create(customerRegister);
        }
    }
}
