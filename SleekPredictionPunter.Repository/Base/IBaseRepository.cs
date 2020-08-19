using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.Repository.Base
{
	public interface IBaseRepository<T> where T : class
	{
		Task<long> Insert(T phoneOwner, bool savechage = true);
		Task<T> Inserts(T model, bool savechage = true);
		Task Update(T owner, bool savechage = true) ;

		Task Delete(T owner, bool savechage = true) ;

		Task<T> GetFirstOrDefault(Func<T, bool> predicate);

		Task<IEnumerable<T>> GetAllQueryable(Func<T, bool> predicate = null, int startIndex = 0, int count = int.MaxValue);

		Task<T> GetById(string id);

		Task<T> GetById(long id);

		Task<T> GetById(int id);
		Task<long> SaveChangesAsync();

		#region Obsolete 
		//Task<long> Insert<T>(T phoneOwner, bool savechage = true) where T : class;
		//Task Update<T>(T phoneOwner, bool savechage = true) where T : class;

		//Task Delete<T>(T phoneOwner, bool savechage = true) where T : class;

		//Task<T> GetFirstOrDefault<T>(Func<T, bool> predicate) where T : class;

		//Task<IEnumerable<T>> GetAllQueryable<T>(Func<T, bool> predicate = null, int startIndex = 0, 
		//	int count = int.MaxValue) where T : class;

		//Task<T> GetById<T>(string id) where T : class;

		//Task<T> GetById<T>(long id) where T : class;

		//Task<T> GetById<T>(int id) where T : class;
		//Task<long> SaveChangesAsync();

		#endregion
	}

}
