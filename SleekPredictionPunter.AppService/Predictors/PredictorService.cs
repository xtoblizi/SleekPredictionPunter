using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Repository.Base;

namespace SleekPredictionPunter.AppService.Predictors
{
	public class PredictorService : IPredictorService
	{
		private readonly IBaseRepository<Predictor> _repo;
		public PredictorService(IBaseRepository<Predictor> baseRepository)
		{
			_repo = baseRepository;
		}

		#region Post Delete and Update
		public async Task<long> Insert(Predictor model, bool savechanges = true)
		{
			return await _repo.Insert(model, savechanges);
		}

		public async Task Update(Predictor model, bool savechanges = true)
		{
			await _repo.Update(model, savechanges);
		}
		public async Task Delete(Predictor model, bool savechanges = true)
		{
			await _repo.Delete(model, savechanges);
		}
		#endregion

		#region Get based services
		public async Task<IEnumerable<Predictor>> GetAllQueryable(int? activationstatus = null, 
			int? gender = null, 
			string state = null,string country = null,
			DateTime? startDate = null, DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue)
		{
			state = string.IsNullOrEmpty(state) ? string.Empty : state.ToLower();
			country = string.IsNullOrEmpty(country) ? string.Empty : country.ToLower();

			Func<Predictor, bool> predicate = (x =>
			(activationstatus == null || x.ActivatedStatus == (EntityStatusEnum)activationstatus)
			&& (gender == null || x.Gender == (GenderEnum)gender)
			&& (startDate == null || x.DateCreated >= startDate)
			&& (endDate == null || x.DateCreated <= endDate)
			&& (string.IsNullOrEmpty(state) || x.State == state)
			&& (string.IsNullOrEmpty(country) || x.Country == country)
			&& (endDate == null || x.DateCreated <= endDate));

			return await _repo.GetAllQueryable(predicate, startIndex, count);
		}

		public async Task<Predictor> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		public async Task<Predictor> GetFirstOrDefault(Predictor model)
		{
			model.Email = string.IsNullOrEmpty(model.Email) ? string.Empty : model.Email.ToLower();
			model.Username = string.IsNullOrEmpty(model.Username) ? string.Empty : model.Username.ToLower();
			model.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? string.Empty : model.PhoneNumber.ToLower();

			Func<Predictor, bool> predicate = (x =>
			(string.IsNullOrEmpty(model.Email) || x.Email == model.Email)
			&& (string.IsNullOrEmpty(model.Username) || x.Username == model.Username)
			&& (string.IsNullOrEmpty(model.PhoneNumber) || x.PhoneNumber == model.PhoneNumber));

			return await _repo.GetFirstOrDefault(predicate);
		}

        public async Task<Predictor> GetByUserName(string userName)
        {
			Func<Predictor, bool> predicate = (x => x.Username == userName);
			return await _repo.GetFirstOrDefault(predicate);
        }
        #endregion

    }
}
