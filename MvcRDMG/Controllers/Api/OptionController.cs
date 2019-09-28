using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Helpers;
using MvcRDMG.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace MvcRDMG.Controllers.Api
{
    [Authorize]
    [Route("api/options")]
    public class OptionController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDungeonService _dungeonService;
        private readonly ILogger<OptionController> _logger;

        public OptionController(IDungeonService dungeonService, IMapper mapper, ILogger<OptionController> logger)
        {
            _dungeonService = dungeonService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("")]
        public JsonResult Get()
        {
            var options = _dungeonService.GetUserOptionsWithSavedDungeons(UserHelper.GetUserId(User.Claims));
            var result = _mapper.Map<IEnumerable<OptionViewModel>>(options);
            return Json(result);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]OptionViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dungeonOption = _mapper.Map<OptionModel>(viewmodel);
                    dungeonOption.UserId = UserHelper.GetUserId(User.Claims);
                    _logger.LogInformation("Attempting save dungeon");
                    _dungeonService.AddDungeonOption(dungeonOption);
                    if (_dungeonService.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(_mapper.Map<OptionViewModel>(dungeonOption));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save dungeon", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { ex.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState });
        }
    }
}