using QLabs.Common;
using QLabs.Customer.Domain;
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
                    PreLoad();
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
            services.Remove(id);
        }

        public ServiceItem Get(Guid id)
        {
            return services[id];
        }

        public List<ServiceItem> GetAll()
        {
            return services.Values.ToList();
        }

        public void Update(ServiceItem entity)
        {
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
                DaysOfTheWeek = new int[]
                {
                    ((int)DayOfWeek.Monday), ((int)DayOfWeek.Tuesday), ((int)DayOfWeek.Wednesday), ((int)DayOfWeek.Thursday), ((int)DayOfWeek.Friday)
                }
            };

            ServiceItemRepository.Instance.Add(serviceA);

            // _Service B_ = € 0,24 / working day(monday - friday)
            ServiceItem serviceB = new ServiceItem
            {
                Id = Constants.Service_B_Id,
                Name = "Service B",
                WorkDayPrice = 0.24M,
                DaysOfTheWeek = new int[]
                {
                    ((int)DayOfWeek.Monday), ((int)DayOfWeek.Tuesday), ((int)DayOfWeek.Wednesday), ((int)DayOfWeek.Thursday), ((int)DayOfWeek.Friday)
                }
            };

            ServiceItemRepository.Instance.Add(serviceB);

            // _Service C_ = € 0,4 / day(monday - sunday)
            ServiceItem serviceC = new ServiceItem
            {
                Id = Constants.Service_C_Id,
                Name = "Service C",
                WorkDayPrice = 0.4M,
                DaysOfTheWeek = new int[]
                {
                    ((int)DayOfWeek.Monday), ((int)DayOfWeek.Tuesday), ((int)DayOfWeek.Wednesday), ((int)DayOfWeek.Thursday), ((int)DayOfWeek.Friday),
                    ((int)DayOfWeek.Saturday), ((int)DayOfWeek.Sunday)
                }
            };

            ServiceItemRepository.Instance.Add(serviceC);
        }
    }
}
