using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RDMG.Core.Abstractions.Services;
using RDMG.Core.Abstractions.Services.Models;
using RDMG.Helpers;
using RDMG.Models.Dungeon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RDMG.Controllers.Web
{
    [Authorize]
    public class DungeonController : Controller
    {
        private readonly IDungeonService _dungeonService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DungeonController(IDungeonService dungeonService, IMapper mapper, ILogger<DungeonController> logger)
        {
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
                List = list.Select(om => _mapper.Map<DungeonOptionViewModel>(om)).ToList()
            };

            var dungeons = await _dungeonService.ListUserDungeonsAsync(userId, cancellationToken);
            foreach (var option in model.List)
            {
                option.Dungeons = dungeons.Where(dm => dm.DungeonOptionId == option.Id).Select(dm => _mapper.Map<DungeonViewModel>(dm)).ToList();
            }

            return View(model);
        }

        public async Task<IActionResult> Load(string name, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<DungeonOptionViewModel>(await _dungeonService.GetDungeonOptionByNameAsync(name, UserHelper.GetUserId(User.Claims), cancellationToken));
            model.Dungeons = (await _dungeonService.ListDungeonsByNameAsync(model.DungeonName, cancellationToken)).Select(dm => _mapper.Map<DungeonViewModel>(dm)).ToList();
            ViewData["ReturnUrl"] = Url.Action("Index", "Dungeon");
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
                _logger.LogError(ex, "Error deleting dungeon.");
            }
            return RedirectToAction("Index");
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
                    var dungeon = new DungeonModel();
                    var existingDungeonOption = await _dungeonService.GetDungeonOptionByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
                    if (existingDungeonOption is null)
                    {
                        optionModel.MonsterType = GetMonsterType(model);
                        await _dungeonService.CreateDungeonOptionAsync(optionModel, cancellationToken);
                        var created = await _dungeonService.GetDungeonOptionByNameAsync(optionModel.DungeonName, optionModel.UserId, cancellationToken);
                        dungeon = await GenerateDungeonAsync(optionModel, created.Id);
                        var id = await _dungeonService.AddDungeonAsync(dungeon, cancellationToken);
                        dungeon.Id = id;
                    }
                    else
                    {
                        optionModel.MonsterType = GetMonsterType(model);
                        dungeon = await GenerateDungeonAsync(optionModel, existingDungeonOption.Id);
                        var existingDungeons = await _dungeonService.ListDungeonsByNameAsync(optionModel.DungeonName, cancellationToken);
                        var oldDungeon = existingDungeons.First();
                        dungeon.Id = oldDungeon.Id;
                        await _dungeonService.UpdateDungeonAsync(dungeon, cancellationToken);
                    }
                    return Json(JsonSerializer.Serialize(dungeon));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating dungeon.");
                }
            }
            FillCreateModelDropDowns(model);
            return View(model);
        }

        private async Task<DungeonModel> GenerateDungeonAsync(DungeonOptionModel optionModel, int optionId)
        {
            var dungeon = await _dungeonService.GenerateDungeonAsync(optionModel);
            dungeon.DungeonOptionId = optionId;
            return dungeon;
        }

        private static string GetMonsterType(DungeonOptionCreateViewModel model)
        {
            var monsterType = string.Join(",", model.MonsterType);
            if (model.MonsterType.Length == GetMonsters().Count)
            {
                monsterType = "any";
            }
            if (model.MonsterType.Length == 0)
            {
                monsterType = "none";
            }

            return monsterType;
        }

        public IActionResult Create()
        {
            var model = new DungeonOptionCreateViewModel
            {
                DeadEnd = true,
                Corridor = true,
                ItemsRarity = 1,
                UserId = UserHelper.GetUserId(User.Claims)
            };
            FillCreateModelDropDowns(model);
            return View(model);
        }

        private static void FillCreateModelDropDowns(DungeonOptionCreateViewModel model)
        {
            // TODO get this from db
            model.DungeonSizes = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Small", Value = "20" },
                    new SelectListItem{ Text = "Medium", Value = "32" },
                    new SelectListItem{ Text = "Large", Value = "44"}
                };
            model.DungeonDifficulties = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Easy", Value = "0" },
                    new SelectListItem{ Text = "Medium", Value = "1" },
                    new SelectListItem{ Text = "Hard", Value = "2" },
                    new SelectListItem{ Text = "Deadly", Value = "3"}
                };
            model.PartyLevels = GenerateIntSelectList(1, 21);
            model.PartySizes = GenerateIntSelectList(1, 9);
            model.TreasureValues = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Low", Value = (0.5d).ToString(CultureInfo.InvariantCulture) },
                    new SelectListItem{ Text = "Standard", Value = "1" },
                    new SelectListItem{ Text = "High", Value = (1.5d).ToString(CultureInfo.InvariantCulture) }
                };
            model.ItemsRarities = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Common", Value = "0" },
                    new SelectListItem{ Text = "Unommon", Value = "1", Selected = true  },
                    new SelectListItem{ Text = "Rare", Value = "2" },
                    new SelectListItem{ Text = "Very Rare", Value = "3" },
                    new SelectListItem{ Text = "Legendary", Value = "4"}
                };
            model.RoomDensities = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Low", Value = "20" },
                    new SelectListItem{ Text = "Medium", Value = "30" },
                    new SelectListItem{ Text = "High", Value = "40"}
                };
            model.RoomSizes = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Small", Value = "20" },
                    new SelectListItem{ Text = "Medium", Value = "35" },
                    new SelectListItem{ Text = "Large", Value = "45"}
                };
            model.MonsterTypes = GetMonsters();
            model.TrapPercents = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "None", Value = "0" },
                    new SelectListItem{ Text = "Few", Value = "15" },
                    new SelectListItem{ Text = "More", Value = "30"}
                };
            model.DeadEnds = GetBools();
            model.Corridors = GetBools();
            model.RoamingPercents = new List<SelectListItem>
                {
                    new SelectListItem{ Text = "None", Value = "0" },
                    new SelectListItem{ Text = "Few", Value = "10" },
                    new SelectListItem{ Text = "More", Value = "20"}
                };
            model.Themes = new List<SelectListItem>
                {
                    new SelectListItem{ Text = @Resources.Dungeon.Dark, Value = "0" },
                    new SelectListItem{ Text = @Resources.Dungeon.Light, Value = "1" },
                    new SelectListItem{ Text = @Resources.Dungeon.Minimal, Value = "2"}
                };
        }

        private static List<SelectListItem> GetBools()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Yes", Value = "true", Selected = true },
                    new SelectListItem{ Text = "No", Value = "false" }
                };
        }

        private static List<SelectListItem> GenerateIntSelectList(int from, int to)
        {
            var list = new List<SelectListItem>();
            for (int i = from; i <= to; i++)
            {
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return list;
        }

        private static List<SelectListItem> GetMonsters()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem{ Text = "Aberrations", Value = "aberration", Selected = true },
                    new SelectListItem{ Text = "Beasts", Value = "beast", Selected = true },
                    new SelectListItem{ Text = "Celestials", Value = "celestial", Selected = true},
                    new SelectListItem{ Text = "Constructs", Value = "construct", Selected = true},
                    new SelectListItem{ Text = "Dragons", Value = "dragon", Selected = true},
                    new SelectListItem{ Text = "Elementals", Value = "elemental", Selected = true},
                    new SelectListItem{ Text = "Fey", Value = "fey", Selected = true},
                    new SelectListItem{ Text = "Fiends", Value = "fiend", Selected = true},
                    new SelectListItem{ Text = "Giants", Value = "giant", Selected = true},
                    new SelectListItem{ Text = "Humanoids", Value = "humanoid", Selected = true},
                    new SelectListItem{ Text = "Monstrosities", Value = "monstrosity", Selected = true},
                    new SelectListItem{ Text = "Oozes", Value = "ooze", Selected = true},
                    new SelectListItem{ Text = "Plants", Value = "plant", Selected = true},
                    new SelectListItem{ Text = "Swarm of tiny beasts", Value = "swarm of Tiny beasts", Selected = true},
                    new SelectListItem{ Text = "Undead", Value = "undead", Selected = true}
                };
        }
    }
}
