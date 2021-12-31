using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Helpers;
using MvcRDMG.Models.Dungeon;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
        [HttpGet]
        public async Task<JsonResult> GetJson(string dungeonName, CancellationToken cancellationToken)
        {
            try
            {
                var results = await _dungeonService.GetSavedDungeonByNameAsync(dungeonName, UserHelper.GetUserId(User.Claims), cancellationToken);
                if (results == null)
                    return Json(null);

                return Json(_mapper.Map<OptionViewModel>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get saved dungeon for dungeon {dungeonName}", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error finding dungeon name");
            }

        }
        [HttpPost]
        public async Task<JsonResult> Post(string dungeonName, [FromBody] SavedDungeonViewModel viewmodel, CancellationToken cancellationToken)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveddungeon = _mapper.Map<SavedDungeonModel>(viewmodel);
                    await _dungeonService.AddSavedDungeonAsync(dungeonName, saveddungeon, UserHelper.GetUserId(User.Claims), cancellationToken);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(_mapper.Map<SavedDungeonViewModel>(saveddungeon));
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