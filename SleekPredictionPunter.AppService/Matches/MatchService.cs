using SleekPredictionPunter.Model.Matches;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Matches
{
	public class MatchService : IMatchService
	{
		private readonly IBaseRepository<Match> _repo;
		public MatchService(IBaseRepository<Match> repo)
		{
			_repo = repo;
		}

		public async Task<long> Insert(Match model, bool savechanges = true)
		{
			return await _repo.Insert(model);
		}

		public async Task Update(Match model, bool savechanges = true)
		{
			 await _repo.Update(model);
		}
		public async Task Delete(Match model, bool savechanges = true)
		{
			await _repo.Delete(model, savechanges);
		}

		public async Task<Match> GetDefault(Func<Match, bool> predicate, bool includeSetAsHotPreviewCriteria = false)
		{
			if (predicate == null)
				throw new NotImplementedException("the method requires that a predicate value is passed, " +
					"this is used to filter the records of the map so as to get filtered and fewer data ");

			if(includeSetAsHotPreviewCriteria == false)
				return await _repo.GetFirstOrDefault(predicate);
			else
			{
				var matches = await _repo.GetAllQueryable(predicate, 0, 10);
				var result = matches.OrderByDescending(x => x.DateUpdated).FirstOrDefault(x => x.IsSetAsHotPreview);
				return result;
			}
		}

		public async Task<Match> GetMatchById(long id)
		{
			 return await _repo.GetById(id);
		}

		public async Task<IEnumerable<Match>> GetMatches(Func<Match, bool> predicate = null, 
			int startIndex = 0, int count = int.MaxValue)
		{
			var matches = await _repo.GetAllQueryable(predicate,startIndex,count);
			return matches;
		}
		public async Task<IEnumerable<Match>> GetMatches<OrderByKey>(Func<Match, bool> whereFunc = null, 
			Func<Match, OrderByKey> orderByDescFunc = null, 
			int startIndex = 0, int count = int.MaxValue)
		{
			var result = await _repo.GetAllQueryable(whereFunc: whereFunc, orderByFunc: orderByDescFunc, startIndex, count);
			return result;
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
		~MatchService()
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
