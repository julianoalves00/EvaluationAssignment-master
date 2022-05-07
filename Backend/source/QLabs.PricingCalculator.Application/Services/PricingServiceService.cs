using Newtonsoft.Json;
using QLabs.Common.Domain;
using QLabs.PricingCalculator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace QLabs.Customer.Application.Services
{
    public class PricingServiceService : IPricingServiceService
    {
        ILogger<PricingServiceService> _logger;
        IConfiguration _config;
        public PricingServiceService(ILogger<PricingServiceService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        public decimal CalculatingPrices(CalculatePrice calculatePrice)
        {
            _logger.LogInformation($"CalculatePrice({calculatePrice.CustomerId}, " +
                $"{calculatePrice.StartDate.ToString("yyyy-MM-dd")}, {calculatePrice.FinishDate.ToString("yyyy-MM-dd")})");

            if (calculatePrice.CustomerId == Guid.Empty ||
                calculatePrice.StartDate == DateTime.MinValue ||
                calculatePrice.FinishDate == DateTime.MinValue)
                throw new Exception("Unable to calculate, missing information!");

            // Get customer contracts
            CustomerItem customer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.GetSection("CustumerApiUrl").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                var response = client.GetAsync($"customer/{calculatePrice.CustomerId}");
                response.Wait();

                if (response.IsCompletedSuccessfully && response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseBody = response.Result.Content.ReadAsStringAsync();
                    customer = JsonConvert.DeserializeObject<CustomerItem>(responseBody.Result); 
                }
                else
                {
                    throw new Exception($"Custumer Id={customer.Id} not found!");
                }
            }

            if (customer == null || customer.Contracts?.Count == 0)
                throw new Exception($"Custumer Id={customer.Id} not found or does not have contracts!");

            IEnumerable<DateTime> totalPeriod = Enumerable.Range(0, 1 + calculatePrice.FinishDate.Subtract(calculatePrice.StartDate).Days)
                                        .Select(offset => calculatePrice.StartDate.AddDays(offset));

            decimal total = Calculate(customer, totalPeriod);

            return total;
        }

        private decimal Calculate(CustomerItem customer, IEnumerable<DateTime> totalPeriod)
        {
            decimal returnValue = 0;
            foreach (var contract in customer.Contracts)
            {
                returnValue += CalculateService(contract, totalPeriod, customer.FreeDaysPromotion ?? 0);
            }

            return returnValue;
        }

        private decimal CalculateService(ContractService contract, IEnumerable<DateTime> totalPeriod, int freeDays)
        {
            decimal returnValue = 0;
            decimal discountPercent = 0;
            IEnumerable<DateTime> normalPeriod = totalPeriod;
            IEnumerable<DateTime> descountPeriod = null;

            // use contract start date if newer than min total period
            if (contract.StartDate > totalPeriod.Min())
                totalPeriod = totalPeriod.Where(d => d >= contract.StartDate);

            // Handle the discounts
            if (contract.DiscountPeriod != null)
            {
                discountPercent = contract.DiscountPeriod.Percentage;

                if (contract.DiscountPeriod.StartDate != null && contract.DiscountPeriod.FinishDate != null)
                {
                    DateTime startDescount = contract.DiscountPeriod.StartDate.Value > totalPeriod.Min() ?
                        contract.DiscountPeriod.StartDate.Value : totalPeriod.Min();

                    DateTime finishDescount = contract.DiscountPeriod.FinishDate.Value < totalPeriod.Max() ?
                        contract.DiscountPeriod.FinishDate.Value : totalPeriod.Max();

                    descountPeriod = Enumerable.Range(0, 1 + finishDescount.Subtract(startDescount).Days)
                                        .Select(offset => startDescount.AddDays(offset));

                    normalPeriod = normalPeriod.Where(d => !descountPeriod.Contains(d));
                }
            }

            if(normalPeriod != null && normalPeriod.Count() > 0)
                returnValue = CalculateService(contract, normalPeriod, freeDays, 0);

            if (descountPeriod != null && descountPeriod.Count() > 0)
                returnValue += CalculateService(contract, descountPeriod, freeDays, discountPercent);

            return returnValue;
        }

        private decimal CalculateService(ContractService contract, IEnumerable<DateTime> totalPeriod, int freeDays, decimal discountPercent)
        {
            decimal returnValue = 0;

            IEnumerable<DateTime> period = totalPeriod.Where(d => contract.Service.DaysOfTheWeek.Contains(((int)d.DayOfWeek)));

            // Count days, only valid days of week and substract free days if exists
            int totalDays = totalPeriod
                .Where(d => contract.Service.DaysOfTheWeek.Contains(((int)d.DayOfWeek)))
                .Count()
                + freeDays;

            // Free days bigger than period return 0
            if (totalDays < 1)
                return 0;

            // Get diary value, first try in contract if not exists get in service item
            decimal diaryValue = contract.WorkDayPrice ?? contract.Service.WorkDayPrice;

            returnValue = diaryValue * totalDays;

            // Apply discount if exists
            if (discountPercent > 0)
                returnValue = returnValue - (returnValue * (discountPercent / 100));

            return returnValue;
        }
    }
}
