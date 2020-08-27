using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.OddCategory
{
    public interface IOddCategoryService
    {
		Task<long> Insert(Model.OddCategory model, bool savechanges = true);
		Task Update(Model.OddCategory model, bool savechanges = true);

		Task Delete(Model.OddCategory model, bool savechanges = true);

		Task<IEnumerable<Model.OddCategory>> GetAllQueryable(Func<Model.OddCategory, bool> wherefunc = null,
			Func<Model.OddCategory, DateTime> orderByfunc = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<Model.OddCategory> GetById(long id);
		Task<Model.OddCategory> GetByName(string name);
	}
}
