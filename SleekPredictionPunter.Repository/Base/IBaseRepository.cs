using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.Repository.Base
{
	public interface IBaseRepository<T> : IDisposable where T : class 
	{
		Task<long> Insert(T phoneOwner, bool savechage = true);
		Task<T> Inserts(T model, bool savechage = true);
		Task Update(T owner, bool savechage = true) ;

		Task Delete(T owner, bool savechage = true) ;

		Task<T> GetFirstOrDefault(Func<T, bool> predicate, Func<T, DateTime> orderByfunc = null);

		Task<IEnumerable<T>> GetAllQueryable(Func<T, bool> predicate = null, 
			int startIndex = 0, int count = int.MaxValue);
		Task<IEnumerable<T>> GetAllQueryable<OrderByKey>(
			Func<T, bool> whereFunc = null,
			Func<T, OrderByKey> orderByFunc = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<long> GetCount();

		DbSet<T> ReturnDbSetForQuery();

		//Task<T> GetById(string id);

		Task<T> GetById(long id);

		//Task<T> GetById(int id);
		Task<long> SaveChangesAsync();

		//Task<dynamic> ReturnIncldedResult<T1, T2>(IIncludableQueryable<T1, T2> includeablequery) where T1 : class where T2 : class;

	}

}
