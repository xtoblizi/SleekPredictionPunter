using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.MatchCategories
{
    public interface IMatchCategoryService
    {
		Task<long> Insert(MatchCategory model, bool savechanges = true);
		Task Update(MatchCategory model, bool savechanges = true);

		Task Delete(MatchCategory model, bool savechanges = true);
		 
		Task<IEnumerable<MatchCategory>> GetAllQueryable();

		Task<MatchCategory> GetById(long id);
		Task<MatchCategory> GetByName(string name);
	}
}
