using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.SubscriberPredictorMap
{
	public interface ISubscriberPredictorMap
	{
		Task<long> Insert(SubscriberPredictorMap model, bool savechage = true);
		Task Update(SubscriberPredictorMap model, bool savechage = true);

		Task Delete(SubscriberPredictorMap model, bool savechage = true);

		Task<SubscriberPredictorMap> GetFirstOrDefault(SubscriberPredictorMap model);

		Task<IEnumerable<SubscriberPredictorMap>> GetAllQueryable(int? activationstatus = null,
			int? gender = null,
			string state = null, string country = null,
			DateTime? startDate = null, DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<SubscriberPredictorMap> GetById(long id);
	}
}
