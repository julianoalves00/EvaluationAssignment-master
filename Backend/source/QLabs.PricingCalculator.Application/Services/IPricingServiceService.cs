using QLabs.PricingCalculator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Customer.Application.Services
{
    public interface IPricingServiceService
    {
        Decimal CalculatingPrices(CalculatePrice calculatePrice);
    }
}
