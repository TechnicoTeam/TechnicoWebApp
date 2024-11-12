namespace Technico.Main.Validators
{
    public interface IPropertyValidator
    {

        public string? E9Valid(string E9);
        public List<Guid>? OwnerIdValid(List<Guid> ownerIds);
    }
}
