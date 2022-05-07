using QLabs.Customer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Customer.Application.Services
{
    public interface ICustomerService
    {
        List<CustomerItem> GetAll();
        CustomerItem Get(Guid id);
        CustomerItem Create(CustomerItem customerRegister);
    }
}
