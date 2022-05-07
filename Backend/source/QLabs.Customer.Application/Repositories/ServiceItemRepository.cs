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
                    //PreLoad();
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

        private static void PreLoad()
        {
            // _Service A_ = € 0,2 / working day(monday - friday)
            ServiceItem serviceA = new ServiceItem
            {
                Id = Constants.Service_A_Id,
                Name = "Service A",
                WorkDayPrice = 0.2M,
                DaysOfTheWeek = Constants.WorkDays
            };

            ServiceItemRepository.Instance.Add(serviceA);

            // _Service B_ = € 0,24 / working day(monday - friday)
            ServiceItem serviceB = new ServiceItem
            {
                Id = Constants.Service_B_Id,
                Name = "Service B",
                WorkDayPrice = 0.24M,
                DaysOfTheWeek = Constants.WorkDays
            };

            ServiceItemRepository.Instance.Add(serviceB);

            // _Service C_ = € 0,4 / day(monday - sunday)
            ServiceItem serviceC = new ServiceItem
            {
                Id = Constants.Service_C_Id,
                Name = "Service C",
                WorkDayPrice = 0.4M,
                DaysOfTheWeek = Constants.AllDays
            };

            ServiceItemRepository.Instance.Add(serviceC);
        }
    }
}
