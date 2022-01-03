using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Models.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RDMG.Controllers.Api
{
    [Authorize]
    [Route("api/generate")]
    public class GenerateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDungeonService _dungeonService;
        private readonly ILogger<OptionController> _logger;

        public GenerateController(IDungeonService dungeonService, IMapper mapper, ILogger<OptionController> logger)
        {
            _dungeonService = dungeonService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody] OptionViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = _mapper.Map<OptionModel>(viewmodel);
                    if (await _dungeonService.GenerateAsync(model))
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