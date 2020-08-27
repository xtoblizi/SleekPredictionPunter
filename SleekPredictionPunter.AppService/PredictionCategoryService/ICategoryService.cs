using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionCategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<PredictionCategory>> GetCategories(Func<PredictionCategory, bool> predicate = null,
            Func<PredictionCategory, DateTime> orderByfunc = null,
            int startIndex = 0, int count = int.MaxValue);
        Task<PredictionCategory> GetCategoryById(long id);
        Task<long> CreateCategory(PredictionCategory model);
        Task RemoveCategoryBy(PredictionCategory owner, bool savechage = true);

        Task DeleteForAllCategories<dynamic> (dynamic model);
    }
}
