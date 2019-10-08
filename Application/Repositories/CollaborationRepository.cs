using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Database;
using Application.Models.Entities;
using MongoDB.Driver;

namespace Application.Repositories
{
    public interface ICollaborationRepository
    {
        Task<List<Event>> Get();
        Task<Event> GetById(string id);
        Task<Event> Create(Event @event);
        Task<IOrderedEnumerable<Event>> GetByProjectId(string projectId);
    }

    public class CollaborationRepository : ICollaborationRepository
    {
        private readonly IMongoCollection<Event> _collaborations;

        public CollaborationRepository(IDatabaseContext dbContext)
        {
            if (dbContext.IsConnectionOpen())
                _collaborations = dbContext.Collaborations;
        }

        public async Task<List<Event>> Get()
        {
            return await (await _collaborations.FindAsync(collaboration => true)).ToListAsync();
        }

        public async Task<Event> GetById(string id)
        {
            return await (await _collaborations.FindAsync(collaboration => collaboration.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<Event> Create(Event @event)
        {
            await _collaborations.InsertOneAsync(@event);
            return @event;
        }

        public async Task<IOrderedEnumerable<Event>> GetByProjectId(string projectId)
        {
            var events = await (await _collaborations.FindAsync(@event => @event.ProjectId == projectId)).ToListAsync();
            return events.OrderByDescending(e => e.Timestamp);
        }
    }
}