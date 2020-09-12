using SleekPredictionPunter.Model.BettingPlatform;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.BetPlatforms
{
	public class BetPlatformService : IBetPlatformService
	{
		private readonly IBaseRepository<BetPlanform> _repo;
		public BetPlatformService(IBaseRepository<BetPlanform> baseRepository)
		{
			_repo = baseRepository;
		}

		public async Task<long> Insert(BetPlanform model, bool savechanges = true)
		{
			return await _repo.Insert(model);
		}

		public async Task Update(BetPlanform model, bool savechanges = true)
		{
			 await _repo.Update(model);
		}

		public async Task Delete(BetPlanform model, bool savechanges = true)
		{
			await _repo.Delete(model);
		}
		public async Task<IEnumerable<BetPlanform>> GetAllQueryable(Func<BetPlanform, bool> wherefunc = null, Func<BetPlanform, DateTime> orderByFunc = null, int startIndex = 0, int count = int.MaxValue)
		{
			return await _repo.GetAllQueryable(whereFunc: wherefunc, orderByFunc: orderByFunc, startIndex: startIndex, count: count);
		}

		public async Task<BetPlanform> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		public async Task<BetPlanform> GetByName(string name)
		{
			Func<BetPlanform, bool> predicate = (x => x.BetPlatformName == name);
			Func<BetPlanform, DateTime> orderbyFunc = (x => x.DateCreated);

			return await _repo.GetFirstOrDefault(predicate, orderbyFunc);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~BetPlatformService()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
		    GC.SuppressFinalize(this);
		}
		#endregion

	}
}
