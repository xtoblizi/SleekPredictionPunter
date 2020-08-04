using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
	public class ContactController
	{
		private readonly ILogger<ContactController> _logger;
		public ContactController(ILogger<ContactController> logger)
		{
			_logger = logger;
		}


	}
}
