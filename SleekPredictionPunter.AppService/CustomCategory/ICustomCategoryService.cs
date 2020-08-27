using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SleekPredictionPunter.Model;

namespace SleekPredictionPunter.AppService.CustomCategory
{
    public interface ICustomCategoryService
    {
		Task<long> Insert(Model.CustomCategory model, bool savechanges = true);
		Task Update(Model.CustomCategory model, bool savechanges = true);

		Task Delete(Model.CustomCategory model, bool savechanges = true);

		Task<IEnumerable<Model.CustomCategory>> GetAllQueryable(Func<Model.CustomCategory, bool> wherefunc = null,
			Func<Model.CustomCategory, DateTime> orderByfunc = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<Model.CustomCategory> GetById(long id);
		Task<Model.CustomCategory> GetByName(string name);
	}
}
