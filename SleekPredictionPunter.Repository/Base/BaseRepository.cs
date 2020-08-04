using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.Repository.Base
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		#region SetUp
		private readonly PredictionDbContext _predictionDbContext;
		private DbSet<T> _entity;
		public BaseRepository(PredictionDbContext predictionDbContext)
		{
			_predictionDbContext = predictionDbContext;

			_entity = _predictionDbContext.Set<T>();
		}

		#endregion

		#region Insert Delete and Update
		public async virtual Task<long> Insert(T model, bool savechage = true)
		{
			if (model == null)
				throw new ArgumentNullException("The model passed into the repository is empty and null");

			var result = _entity.AddAsync(model);

			//if the savechange flag is set then the entity was added and a new id was generated
			return savechage? await SaveChangesAsync() : 0 ;
		}

		public async virtual Task Update(T model, bool savechange = true)
		{
			_entity.Update(model);
			
			if(savechange)
				await SaveChangesAsync();
		}

		public async virtual Task Delete(T model, bool savechange = true) 
		{
			_entity.Remove(model);

			if (savechange)
				await SaveChangesAsync();
		}

		public async virtual Task<long> SaveChangesAsync()
		{
			return await _predictionDbContext.SaveChangesAsync();
		}
		#endregion

		#region Get Implementation
		/// <summary>
		/// Get all records of a table relating to the query parameters and paginatin details
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public async virtual Task<IEnumerable<T>> GetAllQueryable(Func<T, bool> predicate = null, 
			int startIndex = 0, int count = int.MaxValue) 
		{
			if(predicate != null)
            {
				var dbSet = _entity.Where(predicate).Skip(startIndex).Take(count).ToList();
				return await Task.FromResult(dbSet);
			} 
			return await _entity.Skip(startIndex).Take(count).ToListAsync(); 
		}

		/// <summary>
		/// Extended methods of the findby method to get entity by id irrespective of the id type
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async virtual Task<T> GetById(string id)
		{
			return await _entity.FindAsync(id);
		}
		public async virtual Task<T> GetById(long id)
		{
			return await _entity.FindAsync(id);
		}
		public async virtual Task<T> GetById(int id)
		{
			return await _entity.FindAsync(id);
		}

		/// <summary>
		/// Get the first of default of a  queried table
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public async virtual Task<T> GetFirstOrDefault(Func<T, bool> predicate)
		{
			var dbSet = _entity.Where(predicate)?.FirstOrDefault();
			return await Task.FromResult(dbSet);
		}
		#endregion

	}
}
