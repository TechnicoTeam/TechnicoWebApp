using System.ComponentModel.DataAnnotations;

namespace Technico.Main.Models;

public class PropertyOwnerBindingModel
{
    public Guid PropertyId { get; set; }

    [Required(ErrorMessage = "VAT number is required")]
    [Display(Name = "Owner VAT Number")]
    public string OwnerVat { get; set; }
}
