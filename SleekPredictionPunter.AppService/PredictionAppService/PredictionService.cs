using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
