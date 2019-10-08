using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using Application.Repositories;
using Application.Utility.ClientLibrary;
using Application.Utility.ClientLibrary.Project;
using Application.Utility.Exception;
using Application.Utility.Models;
using Newtonsoft.Json;

namespace Application.Services
{
    public interface ICollaborationService
    {
        Task<Collaboration> Open(Collaboration collaboration);
        Task<bool> Accept(Collaboration collaboration, string authorizationToken);
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

        public async Task<Collaboration> Open(Collaboration collaboration)
        {
            var project = await _client.GetProjectAsync(collaboration.ProjectId);
            if (project == null) throw new InvalidCollaboration();

            var createdCollaboration = await _collaborationRepository.Create(collaboration);

            return createdCollaboration ??
                throw new InvalidCollaboration();
        }

        public async Task<bool> Accept(Collaboration collaboration, string authorizationToken)
        {
            var project = await _client.GetProjectAsync(collaboration.ProjectId);
            if (project == null) throw new InvalidCollaboration("projectId invalid");

            project.FreelancerId = collaboration.FreelancerId;

            return await _client.UpdateProjectAsync(authorizationToken, project);
        }
    }
}