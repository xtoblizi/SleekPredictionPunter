using SleekPredictionPunter.Model.HomeDataModels;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.HomeWiningPlanPreview
{
	public class WiningPlanPreviewService : IWiningPlanPreviewService
	{
		private readonly IBaseRepository<WinningPlanPreviewSummary> _repo;
		public WiningPlanPreviewService(IBaseRepository<WinningPlanPreviewSummary> baseRepository)
		{
			_repo = baseRepository;
		}

		public async Task Delete(WinningPlanPreviewSummary model, bool savechanges = true)
		{
			await _repo.Delete(model, savechanges);
		}
		public async Task<long> Insert(WinningPlanPreviewSummary model, bool savechanges = true)
		{
			return await _repo.Insert(model, savechanges);
		}

		public async Task Update(WinningPlanPreviewSummary model, bool savechanges = true)
		{
			await _repo.Update(model, savechanges);
		}

		public async Task<IEnumerable<WinningPlanPreviewSummary>> GetAllQueryable(Func<WinningPlanPreviewSummary, bool> predicate = null,
			Func<WinningPlanPreviewSummary, DateTime> orderByFunc = null, int startIndex = 0, int count = int.MaxValue)
		{
			var result = await _repo.GetAllQueryable<DateTime>(whereFunc :predicate, orderByFunc:orderByFunc,startIndex: startIndex,count: count);
			return result;
		}

		public async Task<WinningPlanPreviewSummary> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		public async Task<WinningPlanPreviewSummary> GetFirstOrDefault(Func<WinningPlanPreviewSummary,bool> whereFunc = null,
			Func<WinningPlanPreviewSummary,DateTime> orderByfunc = null)
		{

			return await _repo.GetFirstOrDefault(whereFunc,orderByfunc); 
		}

		
	}
}
