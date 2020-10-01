using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Models
{
    public class DashboardViewModel
    {
        public long NewAgents { get; set; }
        public long NewPredictions { get; set; }
        public long NewSubscribers { get; set; }
        public IEnumerable<Subscriber> AllSubscriber { get; set; }
        public IEnumerable<Agent> AllAgents { get; set; }
        public string TotalRevenueOnSubscription { get; set; }
    }
}
