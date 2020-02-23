using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRDMG.Core.Abstractions.Services;
using MvcRDMG.Core.Abstractions.Services.Models;
using MvcRDMG.Models.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MvcRDMG.Controllers.Api
{
    [Authorize]
    [Route("api/generate")]
    public class GenerateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDungeonGenerator _generator;
        private readonly ILogger<OptionController> _logger;

        public GenerateController(IDungeonGenerator generator, IMapper mapper, ILogger<OptionController> logger)
        {
            _generator = generator;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public JsonResult Post([FromBody]OptionViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<OptionModel>(viewmodel);
                    if (_generator.Generate(model))
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(_mapper.Map<IEnumerable<SavedDungeonViewModel>>(model.SavedDungeons.ToList()));
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
                return Json(new { ex.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState });
        }
    }
}