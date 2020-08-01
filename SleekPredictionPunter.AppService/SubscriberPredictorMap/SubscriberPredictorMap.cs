using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.SubscriberPredictorMap
{
	public class SubscriberPredictorMap : ISubscriberPredictorMap
	{
		private readonly IBaseRepository<SubscriberPredictorMap> _repo;
		public SubscriberPredictorMap(IBaseRepository<SubscriberPredictorMap> baseRepository)
		{
			_repo = baseRepository;
		}

		#region Insert Update and Delete
		public Task<long> Insert(SubscriberPredictorMap model, bool savechage = true)
		{
			throw new NotImplementedException();
		}

		public Task Update(SubscriberPredictorMap model, bool savechage = true)
		{
			throw new NotImplementedException();
		}
		public Task Delete(SubscriberPredictorMap model, bool savechage = true)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Get Requests
		public Task<SubscriberPredictorMap> GetById(long id)
		{
			throw new NotImplementedException();
		}

		public Task<SubscriberPredictorMap> GetFirstOrDefault(SubscriberPredictorMap model)
		{
			throw new NotImplementedException();
		}
		public Task<IEnumerable<SubscriberPredictorMap>> GetAllQueryable(int? activationstatus = null, int? gender = null, string state = null, string country = null, DateTime? startDate = null, DateTime? endDate = null, int startIndex = 0, int count = int.MaxValue)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
