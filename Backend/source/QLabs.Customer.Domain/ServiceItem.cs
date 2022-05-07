using QLabs.Common;
using System;

namespace QLabs.Customer.Domain
{
    public class ServiceItem : BaseEntity
    {
        public string Name { get; set; }
        public decimal WorkDayPrice { get; set; }
        public int[] DaysOfTheWeek { get; set; }
    }
}
