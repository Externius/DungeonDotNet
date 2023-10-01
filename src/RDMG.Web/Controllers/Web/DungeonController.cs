using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Core.Domain;
using RDMG.Web.Helpers;
using RDMG.Web.Models.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Web.Controllers.Web;

[Authorize]
public class DungeonController : Controller
{
    private readonly IOptionService _optionService;
    private readonly IDungeonService _dungeonService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DungeonController(IDungeonService dungeonService,
        IOptionService optionService,
        IMapper mapper,
        ILogger<DungeonController> logger)
    {
        _optionService = optionService;
        _dungeonService = dungeonService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = UserHelper.GetUserId(User.Claims);
        var list = await _dungeonService.GetAllDungeonOptionsForUserAsync(userId, cancellationToken);
        var model = new DungeonListViewModel
        {
            List = list.Select(_mapper.Map<DungeonOptionViewModel>).ToList()
        };

        var dungeons = await _dungeonService.ListUserDungeonsAsync(userId, cancellationToken);
        foreach (var option in model.List)
        {
            option.Dungeons = dungeons.Where(dm => dm.DungeonOptionId == option.Id).Select(_mapper.Map<DungeonViewModel>);
        }

        return View(model);
    }

    public async Task<IActionResult> Load(string name, int level, CancellationToken cancellationToken)
    {
        var id = UserHelper.GetUserId(User.Claims);
        var model = new LoadViewModel
        {
            Theme = string.Empty,
            Option = _mapper.Map<DungeonOptionViewModel>(await _dungeonService.GetDungeonOptionByNameAsync(name, id, cancellationToken)),
            Themes = (await _optionService.ListOptionsAsync(OptionKey.Theme, cancellationToken))
                        .Select(om => new SelectListItem { Text = om.Name, Value = om.Value, Selected = true })
        };

        var dungeons = await _dungeonService.ListUserDungeonsByNameAsync(model.Option.DungeonName, id, cancellationToken);
        if (level != 0)
            dungeons = dungeons.Where(dm => dm.Level == level).ToList();

        model.Option.Dungeons = dungeons.Select(_mapper.Map<DungeonViewModel>);
        ViewData["ReturnUrl"] = Url.Action("Index", "Dungeon");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _dungeonService.DeleteDungeonAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting dungeon.");
        }
        return RedirectToAction("Index");
    }

    public IActionResult Rename(int id, string dungeonName)
    {
        var model = new DungeonRenameViewModel
        {
            Id = id,
            DungeonName = dungeonName,
            UserId = UserHelper.GetUserId(User.Claims)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rename(DungeonRenameViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            var existing = await _dungeonService.GetDungeonOptionByNameAsync(model.NewDungeonName, model.UserId, cancellationToken);
            if (existing is null)
            {
                await _dungeonService.RenameDungeonAsync(model.Id, model.UserId, model.NewDungeonName, cancellationToken);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(nameof(model.NewDungeonName), string.Format(Resources.Error.DungeonExist, model.NewDungeonName));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error renaming dungeon.");
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOption(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _dungeonService.DeleteDungeonOptionAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting dungeon option.");
        }
        return RedirectToAction("Index");
    }

    private async Task<string> GetMonsterTypeAsync(DungeonOptionCreateViewModel model, CancellationToken cancellationToken)
    {
        var monsterType = string.Join(",", model.MonsterType);
        var monsters = await _optionService.ListOptionsAsync(OptionKey.MonsterType, cancellationToken);
        if (model.MonsterType.Length == monsters.Count())
        {
            monsterType = "any";
        }
        if (model.MonsterType.Length == 0)
        {
            monsterType = "none";
        }

        return monsterType;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DungeonOptionCreateViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var optionModel = _mapper.Map<DungeonOptionModel>(model);
                optionModel.MonsterType = await GetMonsterTypeAsync(model, cancellationToken);
                var dungeon = await _dungeonService.CreateOrUpdateDungeonAsync(optionModel, model.AddDungeon, model.Level, cancellationToken);
                return Json(JsonSerializer.Serialize(dungeon));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dungeon.");
            }
        }
        await FillCreateModelDropDownsAsync(model, cancellationToken);
        return View(model);
    }

    public async Task<IActionResult> Create(int optionId, CancellationToken cancellationToken)
    {
        var model = new DungeonOptionCreateViewModel
        {
            DeadEnd = true,
            Corridor = true,
            ItemsRarity = 1,
            UserId = UserHelper.GetUserId(User.Claims),
            TrapPercent = 15,
            Level = 1
        };

        if (optionId != 0)
        {
            var option = await _dungeonService.GetDungeonOptionAsync(optionId, cancellationToken);
            model = _mapper.Map<DungeonOptionCreateViewModel>(option);
            model.AddDungeon = true;
            if (model.MonsterType[0].Equals("any"))
            {
                model.MonsterType = null;
            }
        }

        await FillCreateModelDropDownsAsync(model, cancellationToken);
        return View(model);
    }

    private async Task FillCreateModelDropDownsAsync(DungeonOptionCreateViewModel model, CancellationToken cancellationToken)
    {
        var options = await _optionService.ListOptionsAsync(null, cancellationToken);
        var optionModels = options.ToList();
        model.DungeonSizes = optionModels.Where(om => om.Key == OptionKey.Size).Select(om => new SelectListItem { Text = om.Name, Value = om.Value });
        model.DungeonDifficulties = optionModels.Where(om => om.Key == OptionKey.Difficulty).Select(om => new SelectListItem { Text = om.Name, Value = om.Value });
        model.PartyLevels = GenerateIntSelectList(1, 21);
        model.PartySizes = GenerateIntSelectList(1, 9);
        model.TreasureValues = optionModels.Where(om => om.Key == OptionKey.TreasureValue).Select(om => new SelectListItem { Text = om.Name, Value = om.Value });
        model.ItemsRarities = optionModels.Where(om => om.Key == OptionKey.ItemsRarity).Select(om => new SelectListItem { Text = om.Name, Value = om.Value });
        model.RoomDensities = optionModels.Where(om => om.Key == OptionKey.RoomDensity).Select(om => new SelectListItem { Text = om.Name, Value = om.Value });
        model.RoomSizes = optionModels.Where(om => om.Key == OptionKey.RoomSize).Select(om => new SelectListItem { Text = om.Name, Value = om.Value });
        model.MonsterTypes = optionModels.Where(om => om.Key == OptionKey.MonsterType).Select(om => new SelectListItem { Text = om.Name, Value = om.Value, Selected = true });
        model.TrapPercents = optionModels.Where(om => om.Key == OptionKey.TrapPercent).Select(om => new SelectListItem { Text = om.Name, Value = om.Value, Selected = true });
        model.DeadEnds = GetBool();
        model.Corridors = GetBool();
        model.RoamingPercents = optionModels.Where(om => om.Key == OptionKey.RoamingPercent).Select(om => new SelectListItem { Text = om.Name, Value = om.Value, Selected = true });
        model.Themes = optionModels.Where(om => om.Key == OptionKey.Theme).Select(om => new SelectListItem { Text = om.Name, Value = om.Value, Selected = true });
    }

    private static List<SelectListItem> GetBool()
    {
        return new List<SelectListItem>
        {
            new() { Text = Resources.Common.Yes, Value = "true", Selected = true },
            new() { Text = Resources.Common.No, Value = "false" }
        };
    }

    private static List<SelectListItem> GenerateIntSelectList(int from, int to)
    {
        var list = new List<SelectListItem>();
        for (var i = from; i <= to; i++)
        {
            list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
        }
        return list;
    }
}