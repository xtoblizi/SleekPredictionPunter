using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionAppService
{
    public interface IPredictionService
    {
        Task<IEnumerable<Prediction>> GetPredictions(Func<Prediction, bool> predicate = null, int startIndex = 0, int count = int.MaxValue);
        Task<Prediction> GetPredictionByPredictor(long id);
        Task<long> InsertPrediction(Prediction model);
        Task RemovePredictionBy(Prediction phoneOwner, bool savechage = true);
    }
}
