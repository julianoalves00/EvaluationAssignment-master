using QLabs.Common;
using System;

namespace QLabs.Common.Domain
{
    public class ServiceItem : BaseEntity
    {
        public string Name { get; set; }
        public decimal WorkDayPrice { get; set; }
        public int[] DaysOfTheWeek { get; set; }
    }
}
