﻿using System;
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
    }
}