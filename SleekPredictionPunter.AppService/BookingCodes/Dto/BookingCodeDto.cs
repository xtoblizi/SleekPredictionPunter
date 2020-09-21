using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.BookingCodes.Dto
{
	public class BookingCodeDto: BookingCode
	{
		public string BetPlatformFilePath { get; set; }
	}
}
