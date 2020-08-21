using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.MatchCategories
{
    public class MatchCategoryService : IMatchCategoryService
    {
        private readonly IBaseRepository<MatchCategory> _repo;

        public MatchCategoryService(IBaseRepository<MatchCategory> repo)
        {
            _repo = repo;
        }

        public Task Delete(MatchCategory model, bool savechanges = true)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MatchCategory>> GetAllQueryable()
        {
            return await _repo.GetAllQueryable();
        }

        public async Task<MatchCategory> GetById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<MatchCategory> GetByName(string name)
        {
            Func<MatchCategory, bool> predicate = (x => x.CategoryName == name);
            return await _repo.GetFirstOrDefault(predicate);
        }

        public async Task<long> Insert(MatchCategory model, bool savechanges = true)
        {
            return await _repo.Insert(model, savechanges);
        }

        public async Task Update(MatchCategory model, bool savechanges = true)
        {
            await _repo.Update(model, savechanges);
        }
    }
}
