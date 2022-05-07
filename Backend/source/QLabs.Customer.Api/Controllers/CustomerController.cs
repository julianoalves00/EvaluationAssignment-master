using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLabs.Customer.Application.Services;
using QLabs.Customer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLabs.Customer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _service;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService service)
        {
            _logger = logger; 
            _service = service;
        }

        [HttpGet]
        public IEnumerable<CustomerItem> Get()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public CustomerItem Get(Guid id)
        {
            return _service.Get(id);
        }

        [HttpPost]
        public CustomerItem Create(CustomerItem customerRegister)
        {
            return _service.Create(customerRegister);
        }
    }
}
