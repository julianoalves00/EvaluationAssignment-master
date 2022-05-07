using QLabs.Common;
using System;
using System.Collections.Generic;

namespace QLabs.Common.Domain
{
    public class CustomerItem : BaseEntity
    {
        public string  Name { get; set; }
        public int? FreeDaysPromotion { get; set; }
        public  List<ContractService> Contracts { get; set; }
    }
}
