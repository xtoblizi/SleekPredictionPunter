using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Withdrawal :BaseEntity
	{
		public Withdrawal()
		{
			WithdrawalStatus = WithdrawalStatus.Pending;
		}
		public string AgentUsername { get; set; }

		public decimal Amount { get; set; }

		public WithdrawalStatus WithdrawalStatus { get; set; }
		public string WithdrawalStatusName { get { return WithdrawalStatus.ToString(); } set { value = WithdrawalStatus.ToString(); } }
	}

	public enum WithdrawalStatus
	{
		Pending = 1,
		Approved = 2,
		Cancelled = 3
	}
}
