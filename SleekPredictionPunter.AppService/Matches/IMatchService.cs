using SleekPredictionPunter.Model.Matches;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Matches
{
	public interface IMatchService : IDisposable
	{
	
		Task<long> Insert(Match model, bool savechanges = true);
		Task Update(Match model, bool savechanges = true);
		Task Delete(Match model, bool savechanges = true);
		Task<IEnumerable<Match>> GetMatches(Func<Match, bool> predicate =null, int startIndex = 0, int count = int.MaxValue);
		Task<IEnumerable<Match>> GetMatches<OrderByKey>(Func<Match, bool> whereFunc = null,
			Func<Match, OrderByKey> orderByDescFunc = null,
			int startIndex = 0, int count = int.MaxValue);
		Task<Match> GetMatchById(long id);
		Task<Match> GetDefault(Func<Match, bool> predicate = null, bool includeSetAsHotPreviewCriteria = false);
	}
}
