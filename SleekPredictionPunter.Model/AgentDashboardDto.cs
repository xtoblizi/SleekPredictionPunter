using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
    public class AgentDashboardDto
    {
        public IEnumerable<Subscriber> AllSubscriber { get; set; }
        public IEnumerable<Subcription> Subscription { get; set; }
        public long SubcribrrCount { get; set; }
        public decimal AgentEarnings { get; set; }
        public decimal? AgentWalletBalance { get; set; }
        public string ProcessingMessage { get; set; }
    }
}
