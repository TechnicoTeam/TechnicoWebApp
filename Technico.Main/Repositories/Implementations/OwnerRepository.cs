using Technico.Main.Data;

namespace Technico.Main.Repositories.Implementations
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly TechnicoDbContext _context;

        public OwnerRepository(TechnicoDbContext context)
        {
            _context = context;
        }
    }
}
