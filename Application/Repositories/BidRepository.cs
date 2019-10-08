using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Database;
using Application.Models.Entities;
using MongoDB.Driver;

namespace Application.Repositories
{
    public interface ICollaborationRepository
    {
        Task<List<Collaboration>> Get();
        Task<Collaboration> GetById(string id);
        Task<List<Collaboration>> GetByProject(string projectId);
        Task<List<Collaboration>> GetByFreelancerId(string freelancerId);
        Task<List<Collaboration>> GetByProjectAndFreelancer(string projectId, string freelancerId);
        Task<Collaboration> Create(Collaboration collaboration);
        Task Update(string id, Collaboration collaborationIn);
        Task Remove(Collaboration collaborationIn);
        Task Remove(string id);
    }

    public class CollaborationRepository : ICollaborationRepository
    {
        private readonly IMongoCollection<Collaboration> _collaborations;

        public CollaborationRepository(IDatabaseContext dbContext)
        {
            if (dbContext.IsConnectionOpen())
                _collaborations = dbContext.Collaborations;
        }

        public async Task<List<Collaboration>> Get()
        {
            return await (await _collaborations.FindAsync(collaboration => true)).ToListAsync();
        }

        public async Task<Collaboration> GetById(string id)
        {
            return await (await _collaborations.FindAsync(collaboration => collaboration.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<List<Collaboration>> GetByProject(string projectId)
        {
            return await (await _collaborations.FindAsync(collaboration => collaboration.ProjectId == projectId)).ToListAsync();
        }

        public async Task<List<Collaboration>> GetByFreelancerId(string freelancerId)
        {
            return await (await _collaborations.FindAsync(collaboration => collaboration.FreelancerId == freelancerId)).ToListAsync();
        }

        public async Task<List<Collaboration>> GetByProjectAndFreelancer(string projectId, string freelancerId)
        {
            return await (
                await _collaborations.FindAsync(
                    collaboration => collaboration.ProjectId == projectId && collaboration.FreelancerId == freelancerId)
            ).ToListAsync();
        }

        public async Task<Collaboration> Create(Collaboration collaboration)
        {
            await _collaborations.InsertOneAsync(collaboration);
            return collaboration;
        }

        public async Task Update(string id, Collaboration collaborationIn)
        {
            await _collaborations.ReplaceOneAsync(collaboration => collaboration.Id == id, collaborationIn);
        }

        public async Task Remove(Collaboration collaborationIn)
        {
            await _collaborations.DeleteOneAsync(collaboration => collaboration.Id == collaborationIn.Id);
        }

        public async Task Remove(string id)
        {
            await _collaborations.DeleteOneAsync(collaboration => collaboration.Id == id);
        }
    }
}