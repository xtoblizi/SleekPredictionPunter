
using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.SubcriptionService
{
	public interface ISubscriptionService
	{
		Task<long> Insert(Subcription model, bool savechage = true);
		Task Update(Subcription model, bool savechage = true);

		Task Delete(Subcription model, bool savechage = true);

		Task<Subcription> GetFirstOrDefault(Subcription model);

		//Task<IEnumerable<Subcription>> GetAllQueryable(int? activationstatus = null,
		//	int? gender = null,
		//	string state = null, string country = null,
		//	DateTime? startDate = null, DateTime? endDate = null,
		//	int startIndex = 0, int count = int.MaxValue);

		Task<Subcription> GetById(long id);
		Task<long> GetMonthlySummaryForPredictions();
	}
}
