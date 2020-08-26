using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Matches
{
	public class MatchDto
	{
		public Club	ClubA { get; set; }
		public Club ClubB { get; set; }

		public string MatchCategory { get; set; }
		public string SportCategory { get; set; }

		public DateTime TimeofMatch { get; set; }
		public MatchStatusEnum MatchStatus { get; set; }
	}

	public class Match : BaseEntity
	{
		public string ClubA { get; set; }
		public string ClubALogoPath { get; set; }

		public string ClubB { get; set; }
		public string ClubBLogoPath { get; set; }

		/// <summary>
		/// Categories relating to this match
		/// </summary>
		public string MatchCategory { get; set; }
		public string SportCategory { get; set; }
		
		public DateTime TimeofMatch { get; set; }
		public MatchStatusEnum MatchStatus { get; set; }

		/// <summary>
		/// This is relationship of all predictions that can/ is created from this match.
		/// </summary>
		public virtual ICollection<Prediction> Predictions { get; set; }
		
	}
}
