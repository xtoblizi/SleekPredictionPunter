using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService
{
	public class AgentRefereeMapService : IAgentRefereeMapService
	{
		private readonly IBaseRepository<AgentRefereeMap> _repo;
		private readonly IAgentService _agentService;
		public AgentRefereeMapService( IBaseRepository<AgentRefereeMap> baseRepository, IAgentService agentService)
		{
			_repo = baseRepository;
			_agentService = agentService;
		}

		// DML services
		public async Task<long> Create(AgentRefereeMap model, bool savechage = true)
		{
			return await _repo.Insert(model,savechage);
		}
		public async Task Delete(AgentRefereeMap model, bool savechage = true)
		{
			await _repo.Delete(model, savechage);
		}
		public async Task Update(AgentRefereeMap model, bool savechage = true)
		{
			await _repo.Update(model, savechage);
		}

		public async Task<decimal> CalculateAgentRevenueByRefereerCode(string refereerCode,decimal unitincomebyreferee = 2.000m)
		{
			var refcount = await GetAllAgentRefereeCount(refereerCode);
			return refcount * unitincomebyreferee;
		}

		// Get Services
		public async Task<long> GetAllAgentRefereeCount(string refereerCode)
		{
			Func<AgentRefereeMap, bool> predicate = (x => x.RefereerCode == refereerCode);
			var result = await _repo.GetAllQueryable(predicate);
			return result.Count();
		}

		public async Task<AgentRefereeMap> GetMapById(long id)
		{
			return await _repo.GetById(id);
		}
		public async Task<IEnumerable<AgentRefereeMap>> GetAll(Func<AgentRefereeMap, bool> predicate = null, int startIndex = 0, int count = int.MaxValue)
		{
			return  await _repo.GetAllQueryable(predicate);
		}
	}
}
