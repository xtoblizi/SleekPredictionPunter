using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionAppService
{
    public interface IPredictionService
    {
        Task<IEnumerable<Prediction>> GetPredictions(Func<Prediction, bool> predicate = null, int startIndex = 0, int count = int.MaxValue);
        Task<Prediction> GetPredictionByPredictor(long id);
        Task<long> InsertPrediction(Prediction model);
        Task<Prediction> GetById(long id);
        Task RemovePredictionBy(Prediction phoneOwner, bool savechage = true);
        Task<long> GetMonthlySummaryForPredictions();
        //Task<IEnumerable<Prediction>> GetFreePrediction();
        Task<IEnumerable<Prediction>> GetPaidPrediction();
        Task<IEnumerable<Prediction>> GetPredictionsOrdered(Func<Prediction, bool> whereFunc = null,
             Func<Prediction, DateTime> orderDescFunc = null,
             int startIndex = 0, int count = int.MaxValue);
        Task<IEnumerable<IGrouping<long, Prediction>>> ReturnRelationalData(Func<Prediction, bool> predicate, bool groupByPredicateCategory = false, bool groupByMatchCategory = false, bool groupByCustomCategory = false);
        Task Update(Prediction prediction);
		Task<Prediction> GetFirstOrDefault(Func<Prediction, bool> whereFunc);
	}
}
