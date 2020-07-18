using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.Repository.Base
{
	public interface IBaseRepository<T> where T : class
	{
		Task<long> Insert(T phoneOwner, bool savechage = true);
		Task Update(T phoneOwner, bool savechage = true) ;

		Task Delete(T phoneOwner, bool savechage = true) ;

		Task<T> GetFirstOrDefault(Func<T, bool> predicate);

		Task<IEnumerable<T>> GetAllQueryable(Func<T, bool> predicate = null, int startIndex = 0, int count = int.MaxValue);

		Task<T> GetById(dynamic id);

		Task<T> GetById(long id);

		Task<T> GetById(int id);
		Task<long> SaveChangesAsync();
	}

}
