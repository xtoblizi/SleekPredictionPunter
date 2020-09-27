using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Subscriptions
{
    public class SubscriptionAppService : ISubscriptionAppService
    {
        private readonly IBaseRepository<Subcription> _baseRepository;
        public SubscriptionAppService(IBaseRepository<Subcription> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<long> CreateSubscription(Subcription model)
        {
            //if (model == null) throw new ArgumentNullException("Model threw a null exception");
            var insert = await _baseRepository.Insert(model, true);
            return insert;
        }

        public async Task<Subcription> GetPredicateRecord(Func<Subcription, bool> predicate)
        {
            var result = await _baseRepository.GetFirstOrDefault(predicate,(x=>x.DateCreated));
            return result;
        }

        //getall: consider one to many record(s) as a batch where in,
        //show the user the count indicating the plan/package he/she is mapped to
        //(i.e), when clicked, th records would be revealed to him

        //public async Task<IEnumerable<Subcription>> GetAll()
        //{
        //    var collectionOfSubs = await _baseRepository.GetAllQueryable();

        //    foreach (var item in collectionOfSubs)
        //    {
        //        Func<Subcription,bool> oeoeo = ((x)=>x.Packages.Where(s=>s.PlanName==);
        //    }
        //}

        public async void Delete(Subcription subcription)
        {
            await _baseRepository.Delete(subcription, true);
        }

        public async Task<IEnumerable<Subcription>> GetAll(Func<Subcription, bool> predicate, int startIndex = 0, int count = int.MaxValue)
        {
            return await _baseRepository.GetAllQueryable(predicate, startIndex, count);
        }
    }
}
