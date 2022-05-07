using QLabs.Customer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Customer.Application.Services
{
    public interface IServiceItemService
    {
        List<ServiceItem> GetAll();
        ServiceItem Get(Guid id);
        ServiceItem Create(ServiceItem customerRegister);
    }
}
