using Microsoft.EntityFrameworkCore.Diagnostics;
using SleekPredictionPunter.AppService.TransactionLog;
using SleekPredictionPunter.AppService.Wallet;
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
        public UserManagementAppService(IWalletAppService walletAppService, ITransactionLogAppService transactionLogAppService)
        {
            _transactionLogAppService = transactionLogAppService;
            _walletAppService = walletAppService;
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
    }
}
