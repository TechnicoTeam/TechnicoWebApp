using Technico.Main.Models.Enums;

namespace Technico.Main.DTOs
{
    public class OwnerDtoResponse
    {
        public Guid Id { get; set; }
        required public string Vat { get; set; }
        required public string Firstname { get; set; }
        required public string Lastname { get; set; }
        required public string Email { get; set; }

        required public string Phone { get; set; }

        public string Address { get; set; } = string.Empty; // the user is not required to fill this 
        public TypeOfUser Role { get; set; } = TypeOfUser.User;
        //public List<Property> Properties { get; set; } = []; // I don't know If we need it 
    }
}
