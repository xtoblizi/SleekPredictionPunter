using SleekPredictionPunter.AppService.Dtos;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService
{
	public interface ISubscriberService 
	{
		Task<long> Insert(SubscriberDto phoneOwner, bool savechage = true);
		Task Update(SubscriberDto phoneOwner, bool savechage = true);

		Task Delete(SubscriberDto phoneOwner, bool savechage = true);

		Task<Subscriber> GetFirstOrDefault(SubscriberDto model);

		Task<IEnumerable<Subscriber>> GetAllQueryable(
			int? activationstatus = null, int? gender = null,
			DateTime? startDate = null, DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<Subscriber> GetById(long id);
	}
}
