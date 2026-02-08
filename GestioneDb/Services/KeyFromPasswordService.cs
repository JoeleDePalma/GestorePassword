using GestioneDb.Data;
using Security;

namespace ControllersServices
{
    public class ControllersServices
    {
        private readonly ApplicationDbContext _context;

        public ControllersServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(byte[], string)> KeyFromPassword(string password, int id, string PasswordSalt = null)
        {
            var User = await _context.Users.FindAsync(id);

            var Hash = User.HashedPassword;
            var MasterPasswordSalt = User.PasswordSalt;

            var Verified = HashingService.VerifyPassword(password, Hash, MasterPasswordSalt);

            if (!Verified)
                return (null, null);

            if (PasswordSalt == null)
                PasswordSalt = SecurityServices.GenerateSalt();

            var Key = CryptographyService.DeriveKey(password, PasswordSalt);

            return (Key, PasswordSalt);
        }
    }
}