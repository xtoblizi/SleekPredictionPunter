using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionCategoryService
{
    public class CategoryService: ICategoryService
    {
        private readonly IBaseRepository<PredictionCategory> _repo;
        public CategoryService(IBaseRepository<PredictionCategory> baseRepository)
        {
            _repo = baseRepository;
        }

        public async Task<long> CreateCategory(PredictionCategory model)
        {
            return await _repo.Insert(model);
        }

        public async Task<IEnumerable<PredictionCategory>> GetCategories(Func<PredictionCategory, bool> predicate = null, int startIndex = 0, int count = int.MaxValue)
        {
            return await _repo.GetAllQueryable(predicate, startIndex, count);
        }

        public async Task<PredictionCategory> GetCategoryById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task RemoveCategoryBy(PredictionCategory owner, bool savechage = true)
        {
            await _repo.Delete(owner, savechage);
        }
    }
}
