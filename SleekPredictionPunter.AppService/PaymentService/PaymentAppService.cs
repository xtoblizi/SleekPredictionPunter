using Microsoft.Extensions.Options;
using PayStack.Net;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.Wallets;
using System;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PaymentService
{
	public class PaymentAppService : IPaymentAppService
    {
       
        private readonly AppSettings _appSettings;
        public PaymentAppService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<(TransactionInitializeResponse, WalletModel)> PaystackPaymentOption(WalletModel walletModel, string callbackUrl, ApplicationUser userModel)
        {
            try
            {
                string secretKey = "sk_test_ff43f0891562d8d81cfd17389654a0c11c157258";
                string reference = Guid.NewGuid().ToString();
                var transaction = new PayStackApi(secretKey);

                walletModel.Amount = walletModel.Amount * 100;
                walletModel.TransactionDescription = $"Subscription For {walletModel.TransactionDescription}";
                var response = transaction.Transactions.Initialize(new TransactionInitializeRequest
                {
                    AmountInKobo = (int)walletModel.Amount,
                    Bearer = userModel.FirstName + " " + userModel.LastName,
                    Metadata = walletModel.TransactionDescription,
                    CallbackUrl = callbackUrl,
                    Email = userModel.Email,
                    Currency = "NGN",
                    Reference = reference
                });
                if (response.Status == true)
                {
                    //successfully initialised, save paystack reference
                    //save this ref, to the db record, 
                    walletModel.ReferenceNumber = response.Data.Reference;
                    walletModel.TransactionStatus = TransactionstatusEnum.Success;
                    walletModel.Amount = walletModel.Amount;
                    walletModel.TransactionType = TransactionTypeEnum.Credit;
                    walletModel.MediumPaid = MediumUsedEnum.Paystack;

                    return (response,walletModel);
                }
                return (null,null);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
