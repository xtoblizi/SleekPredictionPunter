using SleekPredictionPunter.Model.TransactionLogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.TransactionLog
{
    public interface ITransactionLogAppService
    {
        Task<IEnumerable<TransactionLogModel>> GetAllTransactionLog(Func<TransactionLogModel, bool> model, int skip = 0, int take = int.MaxValue);
        Task<IEnumerable<TransactionLogModel>> GetAllTransactionLogForAdmin(int skip = 0, int take = int.MaxValue);
        Task<TransactionLogModel> GetTransactionById(long id);
        Task<TransactionLogModel> InsertNewLog(TransactionLogModel model);
        Task  UpdateTransactionLog(TransactionLogModel model);
        Task<TransactionLogModel> GetPredicatedTransactionLog(Func<TransactionLogModel, bool> model);
    }
}