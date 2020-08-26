using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Matches;
using SleekPredictionPunter.Model.PredicationMatchMaps;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionMatchMaps
{
	public class PredictionMatchMapService: IPredicationMatchMapService
	{
		private readonly IBaseRepository<PredictionMatchMap> _repo;
		private readonly IPredictionService _predictionService;
		public PredictionMatchMapService(IBaseRepository<PredictionMatchMap> repo,
			IPredictionService predictionService)
		{
			_predictionService = predictionService;
			_repo = repo;
		}


		public async Task<long> Insert(PredictionMatchMap model, bool savechanges = true)
		{
			return await _repo.Insert(model, savechanges);
		}

		public async Task Update(PredictionMatchMap model, bool savechanges = true)
		{
			await _repo.Update(model, savechanges);
		}
		public async Task Delete(PredictionMatchMap model, bool savechanges = true)
		{
			await _repo.Delete(model, savechanges);
		}
		public async Task<PredictionMatchMap> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		public async Task<IEnumerable<PredictionMatchDto>> GetAllMappedMatchPredictions(
			PredictionMatchMap predicate, 
			MatchStatusEnum? matchStatusEnum = null,
			DateTime? startDate = null,
			DateTime? endDate = null,
			int startIndex = 0, 
			int count = int.MaxValue)
		{
			//Func<PredictionMatchMap, bool> func = (x =>
			// (predicate.MatchId == 0 || x.MatchId == predicate.MatchId) &&
			// (predicate.PredictionId == 0 || x.PredictionId == predicate.PredictionId) &&
			// (matchStatusEnum == null || x.MatchStatus == matchStatusEnum) && 
			// (startDate == null || x.TimeOfMatch >= startDate) &&
			// (endDate == null || x.TimeOfMatch <= endDate));

			//var result = new List<PredictionMatchDto>();
			//var maps = await _repo.GetAllQueryable(func, startIndex, count);
			//IEnumerable<Prediction> predictions = new List<Prediction>();

			//foreach (var item in maps)
			//{
			//	var predication = await _predictionService.GetById(item.PredictionId);
			//	var mapdto = new PredictionMatchDto()
			//	{
			//		DateOfMap = item.DateCreated,
			//		DateTimeUpdated = item.DateUpdated.Value,
			//		PredicationMatchMapId = item.Id,
			//		MatchId = item.MatchId,
			//		Predictions = 
			//	}

			//}

			//result.AddRange(maps.OrderByDescending(x=>x.DateCreated).Select(x=> new PredictionMatchDto
			//{

			//}))


			var notImplementedEx =  new NotImplementedException("This dude has not been implemented");
			throw notImplementedEx;
		}

		public async Task<IEnumerable<PredictionMatchMap>> GetAllQueryable(Func<PredictionMatchMap, bool> predicate, 
			int startIndex = 0, int count = int.MaxValue)
		{
			var maps = await _repo.GetAllQueryable(predicate, startIndex, count);
			return maps;
		}

	
	}
}
