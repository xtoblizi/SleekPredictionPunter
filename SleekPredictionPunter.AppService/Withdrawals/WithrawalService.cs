using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Withdrawals
{
	public class WithrawalService : IWithdrawalService
	{
		private readonly IBaseRepository<Withdrawal> _repo;
		public WithrawalService(IBaseRepository<Withdrawal> repo)
		{
			_repo = repo;
		}

		public async Task<long> Insert(Withdrawal model)
		{
			return await _repo.Insert(model);
		}

		public async Task Update(Withdrawal model)
		{
			await _repo.Update(model);
		}
		public async Task<Withdrawal> GetById(long id)
		{
			return await _repo.GetById(id);
		}

		public async Task<Withdrawal> GetFirstDefault(Func<Withdrawal, bool> predicate)
		{
			return await _repo.GetFirstOrDefault(predicate);
		}

		public async Task<IEnumerable<Withdrawal>> GetWithdrawals(Func<Withdrawal, bool> predicate = null, Func<Withdrawal, DateTime>  orderbyFunc = null, int startIndex = 0, int take = int.MaxValue)
		{
			return await _repo.GetAllQueryable<DateTime>(whereFunc: predicate, orderByFunc: orderbyFunc, startIndex: startIndex, count: take);
		}

		public async Task Delete(Withdrawal model)
		{
			await _repo.Delete(model);
		}
	}
}
