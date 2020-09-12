using SleekPredictionPunter.Model.BettingPlatform;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.BetPlatforms
{
	public interface IBetPlatformService : IDisposable	
	{
		Task<long> Insert(BetPlanform model, bool savechanges = true);
		Task Update(BetPlanform model, bool savechanges = true);

		Task Delete(BetPlanform model, bool savechanges = true);

		//Task<Club> GetFirstOrDefault(Club model);

		Task<IEnumerable<BetPlanform>> GetAllQueryable(Func<BetPlanform, bool> wherefunc = null, Func<BetPlanform, DateTime> orderByFunc = null
			, int startIndex = 0, int count = int.MaxValue);

		Task<BetPlanform> GetById(long id);
		Task<BetPlanform> GetByName(string name);
	}
}
