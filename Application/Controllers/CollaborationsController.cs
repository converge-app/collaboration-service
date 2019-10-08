using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Helpers;
using Application.Models.DataTransferObjects;
using Application.Models.Entities;
using Application.Repositories;
using Application.Services;
using Application.Utility;
using Application.Utility.Exception;
using Application.Utility.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CollaborationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollaborationRepository _collaborationRepository;
        private readonly ICollaborationService _collaborationService;

        public CollaborationsController(ICollaborationService collaborationService, ICollaborationRepository collaborationRepository, IMapper mapper)
        {
            _collaborationService = collaborationService;
            _collaborationRepository = collaborationRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] EventCreationDto eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var createEvent = _mapper.Map<Event>(eventDto);
            try
            {
                var createdEvent = await _collaborationService.Create(createEvent);
                return Ok(createdEvent);
            }
            catch (ProjectNotFound)
            {
                return NotFound(new MessageObj("Project not found"));
            }
            catch (EnvironmentNotSet)
            {
                throw;
            }
            catch (Exception e)
            {
                return BadRequest(new MessageObj(e.Message));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var collaborations = await _collaborationRepository.Get();
            var collaborationDtos = _mapper.Map<IList<EventDto>>(collaborations);
            return Ok(collaborationDtos);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId([FromRoute] string projectId)
        {
            var events = (await _collaborationRepository.GetByProjectId(projectId)).ToList();
            return Ok(events);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var collaboration = await _collaborationRepository.GetById(id);
            var collaborationDto = _mapper.Map<EventDto>(collaboration);
            return Ok(collaborationDto);
        }
    }
}