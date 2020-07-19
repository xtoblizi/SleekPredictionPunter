using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.Dtos
{
    public class SubscriberDto: PersonDto
    {
        private bool istenant = false;
        public SubscriberDto()
        {
            IsTenant = false;
        }
    }
}
