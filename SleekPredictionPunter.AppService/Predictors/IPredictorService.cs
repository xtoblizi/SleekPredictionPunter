using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Predictors
{
	public interface IPredictorService
	{
		Task<long> Insert(Predictor model, bool savechanges = true);
		Task Update(Predictor model, bool savechanges = true);

		Task Delete(Predictor model, bool savechanges = true);

		Task<Predictor> GetFirstOrDefault(Predictor model);

		Task<IEnumerable<Predictor>> GetAllQueryable(int? activationstatus = null,
			int? gender = null,
			string state = null, string country = null,
			DateTime? startDate = null, DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<Predictor> GetById(long id);
	}
}
