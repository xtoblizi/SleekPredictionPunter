using SleekPredictionPunter.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.ThirdPartyAppService
{
    public interface IThirdPartyUsersAppService
    {
        Task<IEnumerable<ThirdPartyUsersModel>> GetThirdPartyUserDetailsByEmail();
        Task<long> InsertNewThirdPartyUser(ThirdPartyUsersModel model);
    }
}