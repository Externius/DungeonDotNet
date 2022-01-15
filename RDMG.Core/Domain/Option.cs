namespace RDMG.Core.Domain
{
    public class Option : BaseEntity
    {
        public OptionKey Key { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
