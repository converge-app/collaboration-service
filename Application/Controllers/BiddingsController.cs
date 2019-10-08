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
        public async Task<IActionResult> OpenCollaboration([FromBody] CollaborationCreationDto collaborationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var createCollaboration = _mapper.Map<Collaboration>(collaborationDto);
            try
            {
                var createdCollaboration = await _collaborationService.Open(createCollaboration);
                return Ok(createdCollaboration);
            }
            catch (UserNotFound)
            {
                return NotFound(new MessageObj("User not found"));
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

        [HttpPut("{collaborationId}")]
        public async Task<IActionResult> AcceptCollaboration([FromHeader] string authorization, [FromRoute] string collaborationId, [FromBody] CollaborationUpdateDto collaborationDto)
        {
            if (collaborationId != collaborationDto.Id)
                return BadRequest(new MessageObj("Invalid id(s)"));

            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            var updateCollaboration = _mapper.Map<Collaboration>(collaborationDto);
            try
            {
                if (await _collaborationService.Accept(updateCollaboration, authorization.Split(' ') [1]))
                    return Ok();
                throw new InvalidCollaboration();
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
            var collaborationDtos = _mapper.Map<IList<CollaborationDto>>(collaborations);
            return Ok(collaborationDtos);
        }

        [HttpGet("freelancer/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByFreelancerId(string id)
        {
            var collaborations = await _collaborationRepository.GetByFreelancerId(id);
            var collaborationsDto = _mapper.Map<CollaborationDto>(collaborations);
            return Ok(collaborationsDto);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var collaboration = await _collaborationRepository.GetById(id);
            var collaborationDto = _mapper.Map<CollaborationDto>(collaboration);
            return Ok(collaborationDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _collaborationRepository.Remove(id);
            }
            catch (Exception e)
            {
                return BadRequest(new MessageObj(e.Message));
            }

            return Ok();
        }
    }
}