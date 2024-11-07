using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Technico.Main.Models;

namespace Technico.Main.Models.Enums;

public class Owner
{
    public Guid Id { get; set; }
    required public string Vat { get; set; }
    required public string Firstname { get; set; }
    required public string Lastname { get; set; }
    required public string Email { get; set; }
    required public string Password { get; set; }

    required public string Phone { get; set; }

    public string Address { get; set; } = string.Empty; // the user is not required to fill this 
    public TypeOfUser Role { get; set; } = TypeOfUser.User;
    public List<Property> Properties { get; set; } = [];
}
