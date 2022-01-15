using RDMG.Core.Domain;

namespace RDMG.Core.Abstractions.Services.Models
{
    public class OptionModel
    {
        public OptionKey Key { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
