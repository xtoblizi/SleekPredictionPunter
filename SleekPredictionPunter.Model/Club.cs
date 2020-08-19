using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class ClubDto 
	{
		public string ClubName { get; set; }
		public string Description { get; set; }
		public IFormFile ClubLogo { get; set; }
	}

	public class Club : BaseEntity
	{
		public string ClubName { get; set; }
		public string Description { get; set; }
		public  string ClubLogRelativePath { get; set; }
	}
}
