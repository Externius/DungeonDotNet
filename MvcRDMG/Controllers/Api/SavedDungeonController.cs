using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Models;
using MvcRDMG.ViewModels;

namespace MvcRDMG.Controllers.Api
{
    [Authorize]
    [Route("api/options/{dungeonName}/saveddungeons")]
    public class SavedDungeonController : Controller
    {
        private IDungeonRepository _repository;
        private ILogger<SavedDungeonController> _logger;

        public SavedDungeonController(IDungeonRepository repository, ILogger<SavedDungeonController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("")]
        public JsonResult GetJson(string dungeonName)
        {
            try
            {
                var results = _repository.GetSavedDungeonByName(dungeonName, User.Identity.Name);
                if (results == null)
                {
                    return Json(null);
                }
                return Json(Mapper.Map<OptionViewModel>(results));
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
                    var saveddungeon = Mapper.Map<SavedDungeon>(viewmodel);
                    _repository.AddSavedDungeon(dungeonName, saveddungeon, User.Identity.Name);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<SavedDungeonViewModel>(saveddungeon));
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