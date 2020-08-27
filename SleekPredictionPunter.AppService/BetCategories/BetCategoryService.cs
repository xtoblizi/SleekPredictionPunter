using SleekPredictionPunter.Model.Categoriess;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.BetCategories
{
	public class BetCategoryService : IBetCategoryService
	{
		private readonly IBaseRepository<BetCategory> _baseRepository;
		public BetCategoryService(IBaseRepository<BetCategory> baseRepository)
		{
			_baseRepository = baseRepository;
		}

		public async Task<long> Insert(BetCategory model, bool savechanges = true)
		{
			return await _baseRepository.Insert(model);
		}

		public async Task Update(BetCategory model, bool savechanges = true)
		{
			await _baseRepository.Update(model);
		}

		public async Task Delete(BetCategory model, bool savechanges = true)
		{
			await _baseRepository.Delete(model);
		}

		public async Task<IEnumerable<BetCategory>> GetAllQueryable(Func<BetCategory, bool> wherefunc = null, 
			Func<BetCategory, DateTime> orderByfunc = null, int startIndex = 0, int count = int.MaxValue)
		{
			return await _baseRepository.GetAllQueryable(wherefunc, orderByfunc, startIndex, count);
		}

		public async Task<BetCategory> GetById(long id)
		{
			return await _baseRepository.GetById(id);
		}

		public async Task<BetCategory> GetByName(string name)
		{
			Func<BetCategory, bool> func = (x => x.BetCategoryName == name);
			return await _baseRepository.GetFirstOrDefault(func);
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
		~BetCategoryService()
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
