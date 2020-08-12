using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.ThirdPartyAppService
{
    public class ThirdPartyUsersAppService : IThirdPartyUsersAppService
    {
        private readonly IBaseRepository<ThirdPartyUsersModel> _baseRepository;

        public ThirdPartyUsersAppService(IBaseRepository<ThirdPartyUsersModel> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<long> InsertNewThirdPartyUser(ThirdPartyUsersModel model)
        {
            var result = await _baseRepository.Insert(model);
            if (result > 0)
            {
                return result;
            }
            return 0;
        }

        public async Task<IEnumerable<ThirdPartyUsersModel>> GetThirdPartyUserDetailsByEmail()
        {
            var result = await _baseRepository.GetAllQueryable();
            return result;
        }

    }
}
