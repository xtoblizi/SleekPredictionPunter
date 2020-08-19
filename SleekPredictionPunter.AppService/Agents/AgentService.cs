using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Agents
{
    public class AgentService: IAgentService
    {
        private readonly IBaseRepository<Agent> _repo;
        public AgentService(IBaseRepository<Agent> baseRepository)
        {
            _repo = baseRepository;
        }

        public async Task<long> CreateAgent(Agent agent)
        {
            try
            {
                if (!string.IsNullOrEmpty(agent.Email))
                {
                    agent.Username = agent.Email;
                }
                if (!string.IsNullOrEmpty(agent.TenantUniqueName))
                {
                    agent.TenantUniqueName = $"{agent.FirstName}-{agent.BrandNameOrNickName}";
                }
                agent.EntityStatus = Model.Enums.EntityStatusEnum.NotActive;
                agent.DateCreated = DateTime.Now;
                agent.IsTenant = true;
                agent.RefererCode = "generateRandomNumberHere";

                return await _repo.Insert(agent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Agent> GetAgentById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<IEnumerable<Agent>> GetAgents(Func<Agent, bool> predicate = null, int startIndex = 0, int count = int.MaxValue)
        {
            return await _repo.GetAllQueryable(predicate, startIndex, count);
        }

        public async Task RemoveAgentById(Agent owner, bool savechage = true)
        {
            await _repo.Delete(owner, savechage);
        }
    }
}
