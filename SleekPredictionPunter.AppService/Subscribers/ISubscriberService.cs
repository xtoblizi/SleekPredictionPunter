using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService
{
	public interface ISubscriberService 
	{
		Task<long> Insert(Subscriber model, bool savechage = true);
		Task Update(Subscriber model, bool savechage = true);

		Task Delete(Subscriber model, bool savechage = true);

		Task<Subscriber> GetFirstOrDefault(Subscriber model);

		Task<IEnumerable<Subscriber>> GetAllQueryable(int? activationstatus = null,
			int? gender = null,
			string state = null, string country = null,
			DateTime? startDate = null, DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<Subscriber> GetById(long id);
		Task<long> GetMonthlySummaryForPredictions();
	}
}
