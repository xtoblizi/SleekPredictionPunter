using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Packages
{
    public class PackageAppService : IPackageAppService
    {
        private readonly IBaseRepository<Package> _repo;
        public PackageAppService(IBaseRepository<Package> package)
        {
            _repo = package;
        }

        public async Task Delete(Package model)
        {
            await _repo.Delete(model, true);
        }

        public async Task<Package> GetPackageById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<IEnumerable<Package>> GetPackages()
        {
            return await _repo.GetAllQueryable();
        }

        public async Task<long> Insert(Package model)
        {
            return await _repo.Insert(model, true);
        }

        public async Task Update(Package model)
        {
            await _repo.Update(model, true);
        }
    }
}
