using Microsoft.Extensions.Options;
using PayStack.Net;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<(TransactionInitializeResponse, TransactionLogModel)> PaystackPaymentOption(TransactionLogModel walletModel, string callbackUrl, ApplicationUser userModel)
        {
            try
            {
                string secretKey = "sk_live_ec2ce7fcefeefb71cb02c017e0a81a9e658cbcbc";
                string reference = Guid.NewGuid().ToString();
                callbackUrl = $"{callbackUrl}?reference={reference}";
                var transaction = new PayStackApi(secretKey);

                walletModel.CurrentAmount = walletModel.CurrentAmount * 100;
                walletModel.TransactionDescription = $"Subscription For {walletModel.TransactionDescription}";
                var response = transaction.Transactions.Initialize(new TransactionInitializeRequest
                {
                    AmountInKobo = (int)walletModel.CurrentAmount,
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
                    walletModel.TransactionStatus = TransactionstatusEnum.Pending;
                    walletModel.TransactionStatusName = TransactionstatusEnum.Pending.ToString();
                    walletModel.TransactionType = TransactionTypeEnum.Credit;
                    walletModel.TransactionTypeName = TransactionTypeEnum.Credit.ToString();
                    walletModel.MediumPaid = MediumUsedEnum.Paystack;
                    walletModel.MediumPaidName = MediumUsedEnum.Paystack.ToString();

                    return await Task.FromResult((response,walletModel));
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
