using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SleekPredictionPunter.AppService.Dtos;
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
		public async Task<long> Insert(SubscriberDto model, bool savechanges = true)
		{
			var item = new Subscriber()
			{
				ActivatedStatus = (EntityStatusEnum)model.ActivatedStatus,
				BrandNameOrNickName = model.BrandNameOrNickName,
				DateCreated = model.DateCreated,
				EntityStatus = (EntityStatusEnum)model.EntityStatus,
				DateUpdated = model.DateUpdated,
				Email = model.Email,
				FirstName = model.FirstName,
				Gender = (GenderEnum)model.Gender,
				IsTenant = model.IsTenant,
				LastName = model.LastName,
				PhoneNumber = model.PhoneNumber,
				Username = model.Username
			};
			return await _repo.Insert(item, savechanges);
		}

		/// <summary>
		/// Update a new Subscriber
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task Update(SubscriberDto model, bool savechanges = true)
		{
			var item = new Subscriber()
			{
				ActivatedStatus = (EntityStatusEnum)model.ActivatedStatus,
				BrandNameOrNickName = model.BrandNameOrNickName,
				DateCreated = model.DateCreated,
				EntityStatus = (EntityStatusEnum)model.EntityStatus,
				DateUpdated = model.DateUpdated,
				Email = model.Email,
				FirstName = model.FirstName,
				Gender = (GenderEnum)model.Gender,
				IsTenant = model.IsTenant,
				LastName = model.LastName,
				PhoneNumber = model.PhoneNumber,
				Username = model.Username
			};
			await _repo.Update(item, savechanges);
		}

		/// <summary>
		/// Delete a new subscriber
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task Delete(SubscriberDto model, bool savechanges = true)
		{
			var item = new Subscriber()
			{
				ActivatedStatus = (EntityStatusEnum)model.ActivatedStatus,
				BrandNameOrNickName = model.BrandNameOrNickName,
				DateCreated = model.DateCreated,
				EntityStatus = (EntityStatusEnum)model.EntityStatus,
				DateUpdated = model.DateUpdated,
				Email = model.Email,
				FirstName = model.FirstName,
				Gender = (GenderEnum)model.Gender,
				IsTenant = model.IsTenant,
				LastName = model.LastName,
				PhoneNumber = model.PhoneNumber,
				Username = model.Username
			};
			await _repo.Delete(item, savechanges);
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
		public async Task<IEnumerable<Subscriber>> GetAllQueryable(int? activationstatus = null,int? gender = null,
			DateTime? startDate = null , DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue)
		{			
			Func<Subscriber, bool> predicate = (x =>
			(activationstatus == null || x.ActivatedStatus == (EntityStatusEnum)activationstatus)
			&& (gender == null || x.Gender == (GenderEnum)gender)
			&& (startDate == null || x.DateCreated >= startDate)
			&& (endDate == null || x.DateCreated <= endDate)
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
		public async Task<Subscriber> GetFirstOrDefault(SubscriberDto item)
		{
			var model = new Subscriber()
			{
				ActivatedStatus = (EntityStatusEnum)item.ActivatedStatus,
				BrandNameOrNickName= item.BrandNameOrNickName,
				DateCreated= item.DateCreated,
				EntityStatus= (EntityStatusEnum)item.EntityStatus,
				DateUpdated= item.DateUpdated,
				Email= item.Email,
				FirstName= item.FirstName, 
				Gender= (GenderEnum)item.Gender,
				IsTenant= item.IsTenant,
				LastName= item.LastName,
				PhoneNumber= item.PhoneNumber, 
				Username= item.Username
			};

			model.Email = string.IsNullOrEmpty(model.Email) ? string.Empty : model.Email.ToLower();
			model.Username = string.IsNullOrEmpty(model.Username) ? string.Empty : model.Username.ToLower();
			model.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? string.Empty : model.PhoneNumber.ToLower();

			Func<Subscriber, bool> predicate = (x =>
			(string.IsNullOrEmpty(model.Email) || x.Email == model.Email)
			&& (string.IsNullOrEmpty(model.Username) || x.Username == model.Username)
			&& (string.IsNullOrEmpty(model.PhoneNumber) || x.PhoneNumber == model.PhoneNumber));

			return await _repo.GetFirstOrDefault(predicate);
		}

	}
}
