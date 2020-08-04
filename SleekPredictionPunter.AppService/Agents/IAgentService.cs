using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Agents
{
    public interface IAgentService
    {
        Task<IEnumerable<Agent>> GetAgents(Func<Agent, bool> predicate = null, int startIndex = 0, int count = int.MaxValue);
        Task<Agent>GetAgentById(long id);
        Task<long>CreateAgent(Agent model);
        Task RemoveAgentById(Agent owner, bool savechage = true);
    }
}
