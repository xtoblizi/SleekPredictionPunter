using SleekPredictionPunter.Model;
using System;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Subscriptions
{
    public interface ISubscriptionAppService
    {
        Task<long> CreateSubscription(Subcription model);
        void Delete(Subcription subcription);
        Task<Subcription> GetPredicateRecord(Func<Subcription, bool> predicate);
        // this man goto the implementation 
    }
}