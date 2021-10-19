using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("getall")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms...");
            var platformItem = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("getbyid")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("--> Getting Platform By Id...");
            var platformItem = _repository.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        [HttpGet("create")]
        public ActionResult<string> CreatePlatform()
        { 

           // Send Sync Message


            var result = "";
            try
            {
               result =  _commandDataClient.SendPlatformCommand();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronuosly: {ex.Message}");
            }
             
              // Send Sync Message
               
              //Dummy Data For RabbitMQ

              PlatformCreateDto platformCreateDto = new PlatformCreateDto(){
                  Name = $"Deneme-Blazor - {Guid.NewGuid().ToString().Substring(0,5)}",
                  Publisher = $"Deneme-microsoft - {Guid.NewGuid().ToString().Substring(0,5)}",
                  Cost = $"Deneme-free - {Guid.NewGuid().ToString().Substring(0,5)}"
              };

              var platformModel = _mapper.Map<Platform>(platformCreateDto); 
              var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

              try{
                   
                   var ppDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                   ppDto.Event = "Platform-Published";
                   _messageBusClient.PublishNewPlatform(ppDto);

              }catch(Exception ex){

                Console.WriteLine($"--> Couldd not send synchronuosly: {ex.Message}");

              }

            return Ok(result);
        }
    }
}