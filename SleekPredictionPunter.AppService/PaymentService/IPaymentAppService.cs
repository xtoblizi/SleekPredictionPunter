using PayStack.Net;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallet;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PaymentService
{
    public interface IPaymentAppService
    {
        Task<(TransactionInitializeResponse, TransactionLogModel)> PaystackPaymentOption(TransactionLogModel walletModel, string callbackUrl, ApplicationUser userModel);
    }
}