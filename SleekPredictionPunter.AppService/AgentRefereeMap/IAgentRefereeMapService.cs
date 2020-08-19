using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService
{
	public interface IAgentRefereeMapService
	{
		Task<IEnumerable<AgentRefereeMap>> GetAll(Func<AgentRefereeMap, bool> predicate = null, int startIndex = 0, int count = int.MaxValue);
		Task<AgentRefereeMap> GetMapById(long id);
		Task<long> Create(AgentRefereeMap model, bool savechage = true); 
		Task Update(AgentRefereeMap model, bool savechage = true);
		Task Delete(AgentRefereeMap model, bool savechage = true);
		Task<long> GetAllAgentRefereeCount(string refereerCode);
		Task<decimal> CalculateAgentRevenueByRefereerCode(string refereerCode, decimal unitincomebyreferee = 2.000m);
	}
}
