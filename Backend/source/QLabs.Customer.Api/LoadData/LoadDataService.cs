﻿using Microsoft.Extensions.Hosting;
using QLabs.Common;
using QLabs.Common.Domain;
using QLabs.Customer.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QLabs.Customer.Api.LoadData
{
    public class LoadDataService : IHostedService
    {
        IServiceItemService _serviceItem;
        public LoadDataService(IServiceItemService serviceItem)
        {
            _serviceItem = serviceItem;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.FromResult(LoadServices());
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }

        private bool LoadServices()
        {
            // _Service A_ = € 0,2 / working day(monday - friday)
            ServiceItem serviceA = new ServiceItem
            {
                Id = Constants.Service_A_Id,
                Name = "Service A",
                WorkDayPrice = 0.2M,
                DaysOfTheWeek = Constants.WorkDays
            };

            _serviceItem.Create(serviceA);

            // _Service B_ = € 0,24 / working day(monday - friday)
            ServiceItem serviceB = new ServiceItem
            {
                Id = Constants.Service_B_Id,
                Name = "Service B",
                WorkDayPrice = 0.24M,
                DaysOfTheWeek = Constants.WorkDays
            };

            _serviceItem.Create(serviceB);

            // _Service C_ = € 0,4 / day(monday - sunday)
            ServiceItem serviceC = new ServiceItem
            {
                Id = Constants.Service_C_Id,
                Name = "Service C",
                WorkDayPrice = 0.4M,
                DaysOfTheWeek = Constants.AllDays
            };

            _serviceItem.Create(serviceC);

            return true;
        }
    }
}