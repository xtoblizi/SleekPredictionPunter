using PayStack.Net;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.Wallets;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PaymentService
{
    public interface IPaymentAppService
    {
        Task<(TransactionInitializeResponse, WalletModel)> PaystackPaymentOption(WalletModel walletModel, string callbackUrl, ApplicationUser userModel);
    }
}