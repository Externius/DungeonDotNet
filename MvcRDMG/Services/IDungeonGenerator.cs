using System.Collections.Generic;
using MvcRDMG.Generator.Models;
using MvcRDMG.Models;
using MvcRDMG.ViewModels;

namespace MvcRDMG.Services
{
    public interface IDungeonGenerator
    {
        bool Generate(OptionViewModel model);
    }
}