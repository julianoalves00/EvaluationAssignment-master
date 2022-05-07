using QLabs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Common.Domain
{
    public class ContractService : BaseEntity
    {
        public Guid CustumerId { get; set; }
        public Guid ServiceId { get; set; }
        public ServiceItem Service { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? WorkDayPrice { get; set; }
        public DiscountPeriod? DiscountPeriod { get; set; }
    }
}
