using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace SleekPredictionPunter.AppService.PredictionAppService
{
    public class PredictionService: IPredictionService
    {
        private readonly IBaseRepository<Prediction> _repo;
        public PredictionService(IBaseRepository<Prediction> baseRepository)
        {
            _repo = baseRepository;
        }

        public async Task<long> InsertPrediction(Prediction model)
        {
           return await _repo.Insert(model);
        }

        public async Task<Prediction> GetPredictionByPredictor(long id)
        {
           return await _repo.GetById(id);
        }

        public async Task<IEnumerable<Prediction>> GetPredictions(Func<Prediction, bool> predicate = null, int startIndex = 0, int count = int.MaxValue)
        {
            return await _repo.GetAllQueryable(predicate);
        }

        public async Task RemovePredictionBy(Prediction phoneOwner, bool savechage = true)
        {
            await _repo.Delete(phoneOwner, savechage);
        }

        public async Task<long> GetMonthlySummaryForPredictions()
        {
            var dateFrom = DateTime.Now;
            var firstDayOfTheMonth = new DateTime(dateFrom.Year, dateFrom.Month, 1);
            var dateTo = DateTime.Now;

            var getAllPrediction = await _repo.GetAllQueryable();

            var filter = getAllPrediction.Where(x => x.DateCreated >= firstDayOfTheMonth && x.DateCreated <= dateTo);
            return filter.LongCount();
        }

        public async Task<IEnumerable<Prediction>> GetFreePrediction()
        {
            var predictions = await GetPredictions();
            var getAllFreePrediction = predictions
                .Where(x => x.CustomCategory.IsFree == true);
             

            getAllFreePrediction.GroupBy(z => z.PredictionCategory.CategoryName);
            return getAllFreePrediction.ToList();
        }

        public Task<IEnumerable<Prediction>> GetPaidPrediction()
        {
            throw new NotImplementedException();
        }

        
        public async Task<IEnumerable<IGrouping<long, Prediction>>> ReturnRelationalData(Func<Prediction,bool> predicate,bool groupByPredicateCategory = false, bool groupByMatchCategory = false, bool groupByCustomCategory = false)
        {
            var result = await _repo.GetAllQueryable(predicate);
            IEnumerable<IGrouping<long, Prediction>> finalResult = null;
            if (groupByPredicateCategory)
            {
                 finalResult = result.GroupBy(x => x.PredictionCategoryId);
            }
            else if(groupByMatchCategory)
            {
                 finalResult = result.GroupBy(x => x.MatchCategoryId);
            }
            else if(groupByCustomCategory)
            {
               finalResult = result.GroupBy(x => x.CustomCategoryId);
            }

            return finalResult;
        }



    }
}
