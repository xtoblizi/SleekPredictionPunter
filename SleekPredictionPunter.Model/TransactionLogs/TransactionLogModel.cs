using Microsoft.EntityFrameworkCore.Storage;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.Wallets;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.TransactionLogs
{
    public class TransactionLogModel : BaseEntity
    {
        public string UserEmailAddress { get; set; }
        public RoleEnum UserRole { get; set; }
        public long PlanId { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public MediumUsedEnum? MediumPaid { get; set; }
        public string MediumPaidName { get; set; }
        public decimal CurrentAmount { get; set; }
        public string TransactionDescription { get; set; }
        public decimal? LastAmountTransacted { get; set; }
        public DateTime? DateTimeOfLastTransacted { get; set; }
        public TransactionstatusEnum TransactionStatus { get; set; }
        public string TransactionStatusName { get; set; }
        public string ReferenceNumber { get; set; }
        public string ErrorDescription { get; set; }

    }
}
