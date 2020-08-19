using SleekPredictionPunter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Wallet
{
    public class WalletModel:BaseEntity
    {
        public string UserEmailAddress { get; set; }
        public RoleEnum UserRole { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public MediumUsedEnum? MediumPaid { get; set; }
        public string? MediumPaidName { get; set; }
        public decimal Amount { get; set; }
        public string TransactionDescription { get; set; }
        public decimal? LastAmountTransacted { get; set; }
        public DateTime? DateTimeLastTransacted { get; set; }
        public TransactionstatusEnum TransactionStatus { get; set; }
        public string TransactionStatusName { get; set; }
        public string ReferenceNumber { get; set; }
        public string? ErrorDescription { get; set; }

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
        Failed=2,
        Exception=3,
        InsuffucientFund = 4,
        Cancelled=5,
        None =6
    }
}
