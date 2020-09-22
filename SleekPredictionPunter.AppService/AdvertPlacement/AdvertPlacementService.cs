using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.AdvertPlacements
{
	public class AdvertPlacementService : IAdvertPlacementService
	{
		private readonly IBaseRepository<AdvertPlacement> _repo;
		public AdvertPlacementService(IBaseRepository<AdvertPlacement> baseRepository)
		{
			_repo = baseRepository;
		}

		public async Task<long> Create(AdvertPlacement model,bool saveChanges = true)
		{
			return await _repo.Insert(model,saveChanges);
		}

		public async Task Update(AdvertPlacement model, bool savechage = true)
		{
			await _repo.Update(model, savechage);
		}

		public async Task Delete(AdvertPlacement model, bool savechage = true)
		{
			await _repo.Delete(model, savechage);
		}

		public async Task<IEnumerable<AdvertPlacement>> GetAdvertPlacements(Func<AdvertPlacement, bool> predicate = null,
			Func<AdvertPlacement,DateTime> orderbyFunc= null,
			int startIndex = 0, int count = int.MaxValue)
		{

			return await _repo.GetAllQueryable(whereFunc: predicate, orderByFunc: orderbyFunc,startIndex: startIndex, count: count);
		}

		public async Task<AdvertPlacement> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		public async Task<AdvertPlacement> GetDefault(Func<AdvertPlacement, bool> predicate)
		{
			return await _repo.GetFirstOrDefault(predicate);
		}

		
	}
}
