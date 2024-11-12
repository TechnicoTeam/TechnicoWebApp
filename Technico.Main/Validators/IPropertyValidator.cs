namespace Technico.Main.Validators
{
    public interface IPropertyValidator
    {

        public Task<string?> E9Valid(string E9);
        public Task<List<Guid>> OwnerIdValid(List<Guid> ownerIds);
    }
}
