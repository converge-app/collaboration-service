using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Models.Entities;
using Application.Repositories;
using Application.Utility.ClientLibrary;
using Application.Utility.ClientLibrary.Project;
using Application.Utility.Exception;
using Newtonsoft.Json;

namespace Application.Services
{
    public interface ICollaborationService
    {
        Task<Event> Create(Event createEvent);
    }

    public class CollaborationService : ICollaborationService
    {
        private readonly ICollaborationRepository _collaborationRepository;
        private readonly IClient _client;

        public CollaborationService(ICollaborationRepository collaborationRepository, IClient client)
        {
            _collaborationRepository = collaborationRepository;
            _client = client;
        }

        public async Task<Event> Create(Event createEvent)
        {
            if(JsonConvert.DeserializeObject(createEvent.Content) == null)
                throw new InvalidEvent("Content was not parseable");

            if (await _client.GetProjectAsync(createEvent.ProjectId) == null)
                throw new ProjectNotFound();

            createEvent.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return await _collaborationRepository.Create(createEvent);
        }
    }
}