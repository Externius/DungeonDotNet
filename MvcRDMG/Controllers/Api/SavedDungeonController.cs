using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Helpers;
using MvcRDMG.Models;
using System;
using System.Net;

namespace MvcRDMG.Controllers.Api
{
    [Authorize]
    [Route("api/options/{dungeonName}/saveddungeons")]
    public class SavedDungeonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDungeonService _dungeonService;
        private readonly ILogger<SavedDungeonController> _logger;

        public SavedDungeonController(IDungeonService dungeonService, IMapper mapper, ILogger<SavedDungeonController> logger)
        {
            _dungeonService = dungeonService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("")]
        public JsonResult GetJson(string dungeonName)
        {
            try
            {
                var results = _dungeonService.GetSavedDungeonByName(dungeonName, UserHelper.GetUserId(User.Claims));
                if (results == null)
                {
                    return Json(null);
                }
                return Json(_mapper.Map<OptionViewModel>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get saved dungeon for dungeon {dungeonName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error finding dungeon name");
            }

        }
        [HttpPost("")]
        public JsonResult Post(string dungeonName, [FromBody]SavedDungeonViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveddungeon = _mapper.Map<SavedDungeonModel>(viewmodel);
                    _dungeonService.AddSavedDungeon(dungeonName, saveddungeon, UserHelper.GetUserId(User.Claims));

                    if (_dungeonService.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(_mapper.Map<SavedDungeonViewModel>(saveddungeon));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save dungeon", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save dungeon");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on dungeon");
        }
    }
}