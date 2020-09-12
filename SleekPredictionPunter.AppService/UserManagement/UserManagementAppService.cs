using Microsoft.EntityFrameworkCore.Diagnostics;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.AppService.TransactionLog;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SleekPredictionPunter.AppService.UserManagement
{
    public class UserManagementAppService : IUserManagementAppService
    {
        private readonly ITransactionLogAppService _transactionLogAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly ISubscriberService _subscriberService;
        public UserManagementAppService(IWalletAppService walletAppService, ITransactionLogAppService transactionLogAppService,
            ISubscriberService subscriberService)
        {
            _transactionLogAppService = transactionLogAppService;
            _walletAppService = walletAppService;
            _subscriberService = subscriberService;
        }

        public async Task<IEnumerable<TransactionLogModel>> UserLogs(Func<TransactionLogModel, bool> predicate, int skip = 0, int take = int.MaxValue)
        {
            var getAllLogs = await _transactionLogAppService.GetAllTransactionLog(predicate, skip, take);
            return getAllLogs;
        }
        public async Task<WalletModel> GetUserWalletDetails(Func<WalletModel, bool> predIcate)
        {
            var result = await _walletAppService.GetByUserId(predIcate);
            return result != null ? result : null;
        }
        public async Task<Subscriber> GetSubscriberDetails(Func<Subscriber, bool> predIcate)
        {
            var result = await _subscriberService.GetFirstOrDefault(predIcate);
            return result != null ? result : null;
        }
    }
}
