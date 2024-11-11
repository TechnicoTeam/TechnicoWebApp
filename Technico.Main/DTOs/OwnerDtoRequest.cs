using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs
{
    public class OwnerDtoRequest
    {
        required public string Vat { get; set; }
        required public string Firstname { get; set; }
        required public string Lastname { get; set; }
        required public string Email { get; set; }
        required public string Password { get; set; }

        required public string Phone { get; set; }

        public string Address { get; set; } // the user is not required to fill this 
        public TypeOfUser Role { get; set; }
        public List<Property>? Properties { get; set; }
    }
}
