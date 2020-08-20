using SleekPredictionPunter.Model.Wallet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Wallet
{
    public interface IWalletAppService
    {
        Task<IEnumerable<WalletModel>> GetAllWalletD(Func<WalletModel, bool> predicate, int startIndex = 0, int take = int.MaxValue);
        Task<IEnumerable<WalletModel>> GetAllWalletForAdmin(int startIndex = 0, int take = int.MaxValue);
        Task<WalletModel> GetByUserId(Func<WalletModel, bool> predicate);
        Task<bool> InsertNewAmount(WalletModel model);
    }
}