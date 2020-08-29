using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.UserManagement
{
    public interface IUserManagementAppService
    {
        Task<WalletModel> GetUserWalletDetails(Func<WalletModel, bool> predIcate);
        Task<IEnumerable<TransactionLogModel>> UserLogs(Func<TransactionLogModel, bool> predicate, int skip = 0, int take = int.MaxValue);
        Task<Subscriber> GetSubscriberDetails(Func<Subscriber, bool> predIcate);
    }
}