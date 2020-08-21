using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.CustomCategory
{
    public class CustomCategoryService: ICustomCategoryService
    {
        private readonly IBaseRepository<Model.CustomCategory> _repo;

        public CustomCategoryService(IBaseRepository<Model.CustomCategory> repo)
        {
            _repo = repo;
        }

        public async Task Delete(Model.CustomCategory model, bool savechanges = true)
        {
            await _repo.Delete(model, savechanges);
        }

        public async Task<IEnumerable<Model.CustomCategory>> GetAllQueryable()
        {
            return await _repo.GetAllQueryable();
        }

        public async Task<Model.CustomCategory> GetById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Model.CustomCategory> GetByName(string name)
        {
            Func<Model.CustomCategory, bool> predicate = (x => x.CategoryName == name);
            return await _repo.GetFirstOrDefault(predicate);
        }

        public async Task<long> Insert(Model.CustomCategory model, bool savechanges = true)
        {
            return await _repo.Insert(model, savechanges);
        }

        public async Task Update(Model.CustomCategory model, bool savechanges = true)
        {
            await _repo.Update(model, savechanges);
        }
    }
}
