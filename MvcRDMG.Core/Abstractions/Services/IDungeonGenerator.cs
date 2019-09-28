using MvcRDMG.Core.Abstractions.Services.Models;

namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IDungeonGenerator
    {
        bool Generate(OptionModel model);
    }
}