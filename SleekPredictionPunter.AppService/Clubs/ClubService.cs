using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Clubs
{
    public class ClubService : IClubService
    {
        private readonly IBaseRepository<Club> _repo;
        public ClubService(IBaseRepository<Club> baseRepository)
        {
            _repo = baseRepository;
        }
        public async Task Delete(Club model, bool savechanges = true)
        {
            await _repo.Delete(model, savechanges);
        }

        public async Task<IEnumerable<Club>> GetAllQueryable()
        {
            return await _repo.GetAllQueryable();
        }

        public async Task<Club> GetById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Club> GetByName(string name)
        {
            Func<Club, bool> Predicate = (x => x.ClubName == name);
            return await _repo.GetFirstOrDefault(Predicate);
        }

        public async Task<long> Insert(Club model, bool savechanges = true)
        {
            return await _repo.Insert(model, savechanges);
        }

        public async Task Update(Club model, bool savechanges = true)
        {
            await _repo.Update(model, savechanges);
        }
    }
}
