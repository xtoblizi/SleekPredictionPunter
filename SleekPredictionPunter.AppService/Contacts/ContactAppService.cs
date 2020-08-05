using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Contacts
{
	public class ContactAppService : IContactAppService
	{
		private readonly IBaseRepository<Contact> _repo;
		public ContactAppService(IBaseRepository<Contact> baseRepository)
		{
			_repo = baseRepository;
		}

		/// <summary>
		/// Insert a contact
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task<long> Insert(Contact model, bool savechanges = true)
		{
			try
			{
				return await _repo.Insert(model,savechanges);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Update a contact
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task Update(Contact model, bool savechanges = true)
		{
			try
			{
				await _repo.Update(model, savechanges);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Delete contact 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="savechanges"></param>
		/// <returns></returns>
		public async Task Delete(Contact model, bool savechanges = true)
		{
			await _repo.Update(model, savechanges);
		}

		/// <summary>
		/// Get all contacts based on filter parameters
		/// </summary>
		/// <param name="email"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public async Task<IEnumerable<Contact>> GetAllQueryable(string email = null, DateTime? startDate = null, DateTime? endDate = null, int startIndex = 0, int count = int.MaxValue)
		{
			email = string.IsNullOrEmpty(email) ? string.Empty : email.ToLower();
			
			Func<Contact, bool> predicate = (x =>
			 (startDate == null || x.DateCreated >= startDate)
			&& (endDate == null || x.DateCreated <= endDate)
			&& (string.IsNullOrEmpty(email) || x.Email == email)
			&& (endDate == null || x.DateCreated <= endDate));

			return await _repo.GetAllQueryable(predicate, startIndex, count);
		}

		/// <summary>
		/// Get contact record by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<Contact> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		/// <summary>
		/// Get first or default based on the values passed in the model
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<Contact> GetFirstOrDefault(Contact model)
		{
			model.Email = string.IsNullOrEmpty(model.Email) ? string.Empty : model.Email.ToLower();
			model.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? string.Empty : model.PhoneNumber.ToLower();

			Func<Contact, bool> predicate = (x =>
			(string.IsNullOrEmpty(model.Email) || x.Email == model.Email)
			&& (string.IsNullOrEmpty(model.PhoneNumber) || x.PhoneNumber == model.PhoneNumber));

			return await _repo.GetFirstOrDefault(predicate);
		}


	}
}
