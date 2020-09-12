using SleekPredictionPunter.Model.HomeDataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.HomeWiningPlanPreview
{
	public interface IWiningPlanPreviewService
	{
		Task<long> Insert(WinningPlanPreviewSummary model, bool savechanges = true);
		Task Update(WinningPlanPreviewSummary model, bool savechanges = true);

		Task Delete(WinningPlanPreviewSummary model, bool savechanges = true);

		Task<IEnumerable<WinningPlanPreviewSummary>> GetAllQueryable(
			Func<WinningPlanPreviewSummary, bool> predicate = null,
			Func<WinningPlanPreviewSummary, DateTime> orderByFunc = null,
			int startIndex = 0, int count = int.MaxValue);


		Task<WinningPlanPreviewSummary> GetById(long id);
		Task<WinningPlanPreviewSummary> GetFirstOrDefault(
			Func<WinningPlanPreviewSummary, bool> whereFunc = null,
			Func<WinningPlanPreviewSummary, DateTime> orderByfunc = null);
	}
}
