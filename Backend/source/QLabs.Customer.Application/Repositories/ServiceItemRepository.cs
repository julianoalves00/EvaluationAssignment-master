using QLabs.Common;
using QLabs.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Customer.Application.Repositories
{
    public class ServiceItemRepository
    {
        private static ServiceItemRepository _instance;
        private static Dictionary<Guid, ServiceItem> services = new Dictionary<Guid, ServiceItem>();

        private ServiceItemRepository()
        {
        }

        public static ServiceItemRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceItemRepository();
                }

                return _instance;
            }
        }

        public void Add(ServiceItem entity)
        {
            services.Add(entity.Id, entity);
        }

        public void Delete(Guid id)
        {
            if(services.ContainsKey(id))
                services.Remove(id);
        }

        public ServiceItem Get(Guid id)
        {
            return services.ContainsKey(id) ? services[id] : null;
        }

        public List<ServiceItem> GetAll()
        {
            return services.Values.ToList();
        }

        public void Update(ServiceItem entity)
        {
            if (services.ContainsKey(entity.Id))
                services[entity.Id] = entity;
        }
    }
}
