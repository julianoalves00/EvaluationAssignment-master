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
            if(customers.ContainsKey(id))
                customers.Remove(id);
        }

        public CustomerItem Get(Guid id)
        {
            return customers.ContainsKey(id) ? customers[id] : null;
        }

        public List<CustomerItem> GetAll()
        {
            return customers.Values.ToList();
        }

        public void Update(CustomerItem entity)
        {
            if (customers.ContainsKey(entity.Id))
                customers[entity.Id] = entity;
        }
    }
}
