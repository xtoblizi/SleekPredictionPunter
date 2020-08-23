using Microsoft.AspNetCore.Hosting;
using SleekPredictionPunter.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Subscriptions
{
    public interface ISubscriptionAppService
    {
        Task<long> CreateSubscription(Subcription model);
        void Delete(Subcription subcription);
        Task<Subcription> GetPredicateRecord(Func<Subcription, bool> predicate);
        Task<IEnumerable<Subcription>> GetAll(Func<Subcription, bool> predicate, int startIndex = 0 , int count = int.MinValue);
        // this man goto the implementation 
    }
}