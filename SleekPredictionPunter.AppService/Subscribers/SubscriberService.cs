using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Repository.Base;

namespace SleekPredictionPunter.AppService
{
	public class SubscriberService : ISubscriberService
	{
		private readonly IBaseRepository<Subscriber> _repo;
		public SubscriberService(IBaseRepository<Subscriber> baseRepository)
		{
			_repo = baseRepository;
		}
		/// <summary>
		/// Insert a new subscriber
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task<long> Insert(Subscriber model, bool savechanges = true)
		{
			return await _repo.Insert(model, savechanges);
		}

		/// <summary>
		/// Update a new Subscriber
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task Update(Subscriber model, bool savechanges = true)
		{
			await _repo.Update(model, savechanges);
		}

		/// <summary>
		/// Delete a new subscriber
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task Delete(Subscriber model, bool savechanges = true)
		{
			await _repo.Delete(model, savechanges);
		}

		/// <summary>
		/// Get all method of the subscriber service relating to generalized based properties
		/// </summary>
		/// <param name="tenantUniqueName"></param>
		/// <param name="activationstatus"></param>
		/// <param name="gender"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public async Task<IEnumerable<Subscriber>> GetAllQueryable(int? activationstatus = null,
			int? gender = null,
			string state = null, string country = null,
			DateTime? startDate = null, DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue)
		{
			state = string.IsNullOrEmpty(state) ? string.Empty : state.ToLower();
			country = string.IsNullOrEmpty(country) ? string.Empty : country.ToLower();

			Func<Subscriber, bool> predicate = (x =>
			(activationstatus == null || x.ActivatedStatus == (EntityStatusEnum)activationstatus)
			&& (gender == null || x.Gender == (GenderEnum)gender)
			&& (startDate == null || x.DateCreated >= startDate)
			&& (endDate == null || x.DateCreated <= endDate)
			&& (string.IsNullOrEmpty(state)|| x.State == state)
			&& (string.IsNullOrEmpty(country)|| x.Country == country)
			&& (endDate == null || x.DateCreated <= endDate));

			return await _repo.GetAllQueryable(predicate, startIndex, count);
		}

		/// <summary>
		/// Get 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<Subscriber> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		/// <summary>
		/// Pass in only unique property value(s) that you wish to use to filter out a unique
		/// subscriber from the subscriber records.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public async Task<Subscriber> GetFirstOrDefault(Func<Subscriber, bool> predicate)
		{
			return await _repo.GetFirstOrDefault(predicate);
		}
		public async Task<long> GetMonthlySummaryForPredictions()
		{
			var dateFrom = DateTime.Now;
			var firstDayOfTheMonth = new DateTime(dateFrom.Year, dateFrom.Month, 1);
			var dateTo = DateTime.Now;

			var getAllSubscriber = await _repo.GetAllQueryable();

			var filter = getAllSubscriber.Where(x => x.DateCreated >= firstDayOfTheMonth && x.DateCreated <= dateTo);
			return filter.LongCount();
		}

		public async Task<long> GetCount()
		{
			return await _repo.GetCount();
		}		
		public async Task<long> GetCount(Func<Subscriber, bool> func)
		{
			return await _repo.GetCount(func);
		}
		public async Task<IEnumerable<Subscriber>> GetAllSubscribersByAgentRefcode(Func<Subscriber, bool> predicate, int startIndex = 0, int count = int.MaxValue)
		{
			var getAll = await _repo.GetAllQueryable(predicate, (x=>x.DateCreated),startIndex, count);
			return getAll;
		}
		public async Task<IEnumerable<Subscriber>> GetAllSubscribersByAgentRefcode(Func<Subscriber, bool> predicate, Func<Subscriber,DateTime> orderByFunc,
			int startIndex = 0, int count = int.MaxValue)
		{
			var getAll = await _repo.GetAllQueryable(predicate,orderByFunc,startIndex, count);
			return getAll;
		}

		public async Task<long> GetCountByFilter(Func<Subscriber, bool> func)
		{
			return await _repo.GetCount(func);
		}
	}
}
