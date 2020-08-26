using SleekPredictionPunter.Model.Matches;
using SleekPredictionPunter.Model.PredicationMatchMaps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionMatchMaps
{
	 public interface IPredicationMatchMapService
	{
		Task<long> Insert(PredictionMatchMap model, bool savechanges = true);
		Task Update(PredictionMatchMap model, bool savechanges = true);

		Task Delete(PredictionMatchMap model, bool savechanges = true);

		Task<IEnumerable<PredictionMatchMap>> GetAllQueryable(Func<PredictionMatchMap, bool> predicate, 
			int startIndex = 0, int count = int.MaxValue);
		Task<IEnumerable<PredictionMatchDto>> GetAllMappedMatchPredictions(
			PredictionMatchMap predicate,
			MatchStatusEnum? matchStatusEnum = null,
			DateTime? startDate = null,
			DateTime? endDate = null,
			int startIndex = 0,
			int count = int.MaxValue);

		Task<PredictionMatchMap> GetById(long id);
	}
}
