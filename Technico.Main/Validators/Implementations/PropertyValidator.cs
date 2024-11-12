using Microsoft.IdentityModel.Tokens;
using Technico.Main.Models;

namespace Technico.Main.Validators.Implementations
{
    public class PropertyValidator: IPropertyValidator
    {
        // Check if the E9 is valid 
        public string? E9Valid(string E9)
        {
            if (string.IsNullOrWhiteSpace(E9)){

                return null;
            }
            // Remove all spaces and special characters
            var cleanedE9 = new string(E9.Where(char.IsLetterOrDigit).ToArray());

            // If the cleaned string is empty, return null
            if (string.IsNullOrWhiteSpace(cleanedE9))
            {
                return null;
            }

            return cleanedE9;
        }


        // Check if the List of Owners' Ids are valid
        public List<Guid>? OwnerIdValid( List<Guid> ownerIds)
        {
            if (ownerIds == null || !ownerIds.Any())
            {
                // Return an empty list if the input is null or empty
                return null;
            }

            // Remove Guid.Empty (equivalent of null/empty check for Guid)
            var filteredOwnerIds = ownerIds
                .Where(id => id != Guid.Empty)
                .Distinct() // Remove duplicates
                .ToList();

            return filteredOwnerIds;
        }

    }
}
