using SleekPredictionPunter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Wallets
{
    public class WalletModel:BaseEntity
    {
        public string UserEmailAddress { get; set; }
        public RoleEnum UserRole { get; set; }
        public decimal Amount { get; set; }
        public decimal? LastAmountTransacted { get; set; }
        public DateTime? DateTimeLastTransacted { get; set; }
    }

    public enum TransactionTypeEnum:int
    {
        Debit=1,
        Credit=2
    }
    public enum MediumUsedEnum:int
    {
        Paystack = 1,
        Flutterwave = 2,
        Interswitch = 3,
        QuickTelleer = 4,
        BankTeller = 5
    }

    public enum TransactionstatusEnum
    {
        Success=1,
        Pending = 2,
        Failed=3,
        Exception=4,
        InsuffucientFund = 5,
        Cancelled=6,
        None =7
    }
}
