using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.GeneralUtilsAndServices.JsonHelper
{
	public class JsonHelper
	{
		public static (bool,string) TrySerialize<T>(T data)
		{
			try
			{
				var result = JsonConvert.SerializeObject(data);
				return (true, result);
			}
			catch (Exception)
			{
				return (false, null);
			}
		}

		public static (bool, string) TryToJObject<T>(T data)
		{
			try
			{
				var result = JObject.FromObject(data);
				return  (true,result.ToString());
			}
			catch (Exception)
			{
				return (false,null);
			}
		}

		public static (bool, string) TryToJObjectArray<T>(List<T> data)
		{
			try
			{
				var result = JArray.FromObject(data);
				return (true, result.ToString());
			}
			catch (Exception)
			{
				return (false, default);
			}
		}

		public static (bool,T) TryDeSerialize<T>(string stringifieddata)
		{
			try
			{
				var result = JsonConvert.DeserializeObject<T>(stringifieddata);
				return (true,result);
			}
			catch (Exception)
			{
				return (false,default);
			}
		}
	}
}
