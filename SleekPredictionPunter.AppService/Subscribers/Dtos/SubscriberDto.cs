using SleekPredictionPunter.Model.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.Subscribers.Dtos
{
    public class SubscriberDto : Person
	{
		private bool istenant = false;
		public SubscriberDto()
		{
			IsTenant = false;
		}
	}
}
