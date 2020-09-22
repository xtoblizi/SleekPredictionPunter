using System;
using System.Collections.Generic;
using System.Text;
using SleekPredictionPunter.Model;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.AdvertPlacements
{
	public interface IAdvertPlacementService
	{
        Task<IEnumerable<AdvertPlacement>> GetAdvertPlacements(
            Func<AdvertPlacement, bool> predicate = null,
            Func<AdvertPlacement, DateTime> orderbyFunc = null,
            int startIndex = 0, int count = int.MaxValue);
        Task<AdvertPlacement> GetById(long id);
        Task<long> Create(AdvertPlacement model, bool saveChanges = true);
        Task Delete(AdvertPlacement owner, bool savechage = true);
        //Task<long> GetMonthlySummaryForNewAgents();
        Task<AdvertPlacement> GetDefault(Func<AdvertPlacement, bool> predicate);
        Task Update(AdvertPlacement model, bool savechage = true);

    }
}
