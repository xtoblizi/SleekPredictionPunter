using SleekPredictionPunter.Model.Wallets;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Wallet
{
    public class WalletAppService : IWalletAppService
    {
        private readonly IBaseRepository<WalletModel> _baseRepository;
        public WalletAppService(IBaseRepository<WalletModel> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> InsertNewAmount(WalletModel model)
        {
            var result = await _baseRepository.Insert(model);
            return result > 0 ? true : false;
        }

        public async Task<IEnumerable<WalletModel>> GetAllWalletForAdmin(int startIndex = 0, int take = int.MaxValue)
        {
            var result = await _baseRepository.GetAllQueryable(startIndex: startIndex, count: take);
            return result;
        }

        public async Task<WalletModel> GetByUserId(Func<WalletModel, bool> predicate)
        {
            var getById = await _baseRepository.GetFirstOrDefault(predicate);
            return getById;
        }

        public async Task<IEnumerable<WalletModel>> GetAllWalletD(Func<WalletModel, bool> predicate, int startIndex = 0, int take = int.MaxValue)
        {
            var result = await _baseRepository.GetAllQueryable(predicate, startIndex: startIndex, count: take);
            return result;
        }
    }
}
