using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Models;
using MvcRDMG.Services;
using MvcRDMG.ViewModels;
using Newtonsoft.Json;

namespace MvcRDMG.Controllers.Api
{
    [Authorize]
    [Route("api/generate")]
    public class GenerateController : Controller
    {
        private IDungeonGenerator _generator;
        private ILogger<OptionController> _logger;

        public GenerateController(IDungeonGenerator generator, ILogger<OptionController> logger)
        {
            _generator = generator;
            _logger = logger;
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]OptionViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_generator.Generate(viewmodel))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<IEnumerable<SavedDungeonViewModel>>(viewmodel.SavedDungeons.ToList()));
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return Json(new { Message = "Internal Error" });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to generate dungeon", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}