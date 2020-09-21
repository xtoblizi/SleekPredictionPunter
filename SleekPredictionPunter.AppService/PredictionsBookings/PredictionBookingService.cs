using SleekPredictionPunter.Model.PredictionBookings;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionsBookings
{
	public class PredictionBookingService : IPredictionBookingService
	{
		private readonly IBaseRepository<PredictionBooking> _repo;
		public PredictionBookingService(IBaseRepository<PredictionBooking> repo)
		{
			_repo = repo;
		}


		public async Task<long> Insert(PredictionBooking model, bool savechanges = true)
		{
			return await _repo.Insert(model, savechanges);
		}

		public async Task Update(PredictionBooking model, bool savechanges = true)
		{
			await _repo.Update(model, savechanges);
		}
		public async Task Delete(PredictionBooking model, bool savechanges = true)
		{
			await _repo.Delete(model, savechanges);
		}

		public async Task<PredictionBooking> GetBookingById(long id)
		{
			return await _repo.GetById(id);
		}
		public async Task<PredictionBooking> GetDefault(Func<PredictionBooking, bool> predicate = null,
			bool includeSetAsHotPreviewCriteria = false)
		{
			return await _repo.GetFirstOrDefault(predicate);
		}

		public async Task<IEnumerable<PredictionBooking>> GetBookings<OrderByKey>(Func<PredictionBooking, bool> whereFunc = null,
			Func<PredictionBooking, OrderByKey> orderByDescFunc = null, int startIndex = 0, int count = int.MaxValue)
		{
			return await _repo.GetAllQueryable<OrderByKey>(whereFunc: whereFunc, orderByFunc: orderByDescFunc,  startIndex:startIndex, count:count);
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
		~PredictionBookingService()
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
