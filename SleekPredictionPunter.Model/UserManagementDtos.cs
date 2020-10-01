using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
    public class PredicateForTransactionLog
    {
        public IEnumerable<TransactionLogModel> TransactionLog { get; set; }
        public WalletModel WalletModel { get; set; }
        public Subscriber SubscriberModel{ get; set; }

        public string CurrentPlan { get; set; }
        public string CurrentPlanExpirationDate { get; set; }
    }
}
