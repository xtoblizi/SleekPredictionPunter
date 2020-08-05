using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SleekPredictionPunter.GeneralUtilsAndServices
{
	public class EnumResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
	public static class EnumHelper
	{
		public static List<EnumResult> GetEnumResults<T>()
		{
			var enumList = new List<EnumResult>();
			foreach (var item in Enum.GetValues(typeof(T)))
			{

				enumList.Add(new EnumResult()
				{
					Id = (int)item,
					Name = GetDescription<T>((T)item).ToString(),
				});
			}
			return enumList;
		}

		public static string GetDescription(this Enum value)
		{
			return
				value
					.GetType()
					.GetMember(value.ToString())
					.FirstOrDefault()
					?.GetCustomAttribute<DescriptionAttribute>()
					?.Description
					?.ToString();
		}

		// Get the Description of the enum value by Generic T enum value
		public static string GetDescription<T>(T value)
		{
			return
				value
					.GetType()
					.GetMember(value.ToString())
					.FirstOrDefault()
					?.GetCustomAttribute<DescriptionAttribute>()
					?.Description
					?.ToString();
		}


	}
}
