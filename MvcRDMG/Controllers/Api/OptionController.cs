using System;
using System.Collections.Generic;
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
    [Route("api/options")]
    public class OptionController : Controller
    {
        private IDungeonRepository _repository;
        private ILogger<OptionController> _logger;

        public OptionController(IDungeonRepository repository, ILogger<OptionController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("")]
        public JsonResult Get()
        {
            var options = _repository.GetUserOptionsWithSavedDungeons(User.Identity.Name);
            var result = Mapper.Map<IEnumerable<OptionViewModel>>(options);
            return Json(result);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]OptionViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dungeonOption = Mapper.Map<Option>(viewmodel);
                    dungeonOption.UserName = User.Identity.Name;
                    _logger.LogInformation("Attempting save dungeon");
                    _repository.AddDungeonOption(dungeonOption);
                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<OptionViewModel>(dungeonOption));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save dungeon", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}