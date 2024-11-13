using Technico.Main.Models.Enums;

namespace Technico.Main.Models
{
    public class AdminPropertyCreateModel
    {
        public string Address { get; set; } = string.Empty;
        public int ConstructionYear { get; set; } // this could be an int
        public string E9 { get; set; } = string.Empty;
        public TypeOfProperty Type { get; set; }
        public string Vat { get; set; } = string.Empty;
    }
}
