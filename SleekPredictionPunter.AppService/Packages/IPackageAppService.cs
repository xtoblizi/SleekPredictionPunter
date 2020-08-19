using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Packages;

namespace SleekPredictionPunter.AppService.Packages
{
    public interface IPackageAppService
    {
        Task<IEnumerable<Package>>GetPackages();
        Task<Package> GetPackageById(long id);
        Task<long> Insert(Package model);
        Task Update(Package model);
        Task Delete(Package model);
    }
}
