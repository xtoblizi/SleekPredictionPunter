using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.TransactionLog
{
    public class TransactionLogAppService : ITransactionLogAppService
    {
        private readonly IBaseRepository<TransactionLogModel> _baseRepository;

        public TransactionLogAppService(IBaseRepository<TransactionLogModel> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<TransactionLogModel> InsertNewLog(TransactionLogModel model)
        {
            var insert = await _baseRepository.Inserts(model, true);
            return insert;
        }

        public async Task UpdateTransactionLog(TransactionLogModel model)
        {
            await _baseRepository.Update(model, true);
        }

        public async Task<IEnumerable<TransactionLogModel>> GetAllTransactionLogForAdmin(int skip = 0, int take = int.MaxValue)
        {
            var result = await _baseRepository.GetAllQueryable(startIndex: skip, count: take);
            return result;
        }

        public async Task<TransactionLogModel> GetTransactionById(long id)
        {
            var getById = await _baseRepository.GetById(id);
            return getById;
        }

        public async Task<IEnumerable<TransactionLogModel>> GetAllTransactionLog(Func<TransactionLogModel, bool> model, int skip = 0, int take = int.MaxValue)
        {
            var result = await _baseRepository.GetAllQueryable(model, skip, take);
            return result;
        }

        public async Task<TransactionLogModel> GetPredicatedTransactionLog(Func<TransactionLogModel, bool> model)
        {
            var result = await _baseRepository.GetFirstOrDefault(model);
            return result;
        }
    }
}
