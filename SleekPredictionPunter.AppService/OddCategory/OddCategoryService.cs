using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.OddCategory
{
    public class OddCategoryService: IOddCategoryService
    {
        private readonly IBaseRepository<Model.OddCategory> _repo;

        public OddCategoryService(IBaseRepository<Model.OddCategory> repo)
        {
            _repo = repo;
        }

        public async Task Delete(Model.OddCategory model, bool savechanges = true)
        {
            await _repo.Delete(model, savechanges);
        }

        public async Task<IEnumerable<Model.OddCategory>> GetAllQueryable(Func<Model.OddCategory, bool> wherefunc = null,
            Func<Model.OddCategory,DateTime> orderByfunc=null,
            int startIndex = 0, int count = int.MaxValue)
        {
            return await _repo.GetAllQueryable(wherefunc, orderByfunc, startIndex,count);
        }

        public async Task<Model.OddCategory> GetById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Model.OddCategory> GetByName(string name)
        {
            Func<Model.OddCategory, bool> predicate = (x => x.CategoryName == name);
            return await _repo.GetFirstOrDefault(predicate);
        }

        public async Task<long> Insert(Model.OddCategory model, bool savechanges = true)
        {
            return await _repo.Insert(model, savechanges);
        }

        public async Task Update(Model.OddCategory model, bool savechanges = true)
        {
            await _repo.Update(model, savechanges);
        }
    }
}
