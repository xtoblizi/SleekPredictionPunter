using Microsoft.AspNetCore.Mvc.Diagnostics;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Subscriptions
{
    public class SubscriptionAppService
    {
        private readonly IBaseRepository<Subcription> _baseRepository;
        public SubscriptionAppService(IBaseRepository<Subcription> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<long> CreateSubscription(Subcription model)
        {
            if (model == null) throw new ArgumentNullException("Model threw a null exception");
            var insert = await _baseRepository.Insert(model, true);
            return insert;
        }

    }
}
