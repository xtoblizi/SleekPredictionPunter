using SleekPredictionPunter.Model.Categoriess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.BetCategories
{
	public interface IBetCategoryService : IDisposable
	{
		Task<long> Insert(BetCategory model, bool savechanges = true);
		Task Update(BetCategory model, bool savechanges = true);

		Task Delete(BetCategory model, bool savechanges = true);

		Task<IEnumerable<BetCategory>> GetAllQueryable(Func<BetCategory, bool> wherefunc = null,
			Func<BetCategory, DateTime> orderByfunc = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<BetCategory> GetById(long id);
		Task<BetCategory> GetByName(string name);
	}
}
