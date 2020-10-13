using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Withdrawals
{
	public interface IWithdrawalService
	{
        Task<Withdrawal> GetFirstDefault(Func<Withdrawal, bool> predicate);
        Task<Withdrawal> GetById(long id);      
        Task<long> Insert(Withdrawal model);
        Task Update(Withdrawal model);
        Task<IEnumerable<Withdrawal>> GetWithdrawals(Func<Withdrawal, bool> predicate = null, Func<Withdrawal, DateTime> orderbyFunc = null, int startIndex = 0, int take = int.MaxValue);
        Task Delete(Withdrawal model);
        Task<long> GetCount();
    }
}
