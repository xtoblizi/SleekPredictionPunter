using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.Model.Packages
{
    public class Package: BaseEntity
    {
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
		//public string PackageParameters { get; set; }
		public virtual ICollection<Prediction> Predictions { get; set; }
	}
}
