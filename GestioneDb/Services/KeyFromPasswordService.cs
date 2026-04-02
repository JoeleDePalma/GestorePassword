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

        /// <summary>
        /// Validates the provided password against the user's stored hash and, if valid,
        /// derives an encryption key using the given or newly generated salt
        /// </summary>
        /// <param name="password">The password provided by the user </param>
        /// <param name="id">The ID of the user requesting the operation </param>
        /// <param name="PasswordSalt">
        /// The salt used to derive the encryption key. If null, a new salt is generated
        /// </param>
        /// <returns>
        /// A tuple containing the derived encryption key and the salt used.
        /// Returns <c>(null, null)</c> if the password validation fails
        /// </returns>
        public async Task<(byte[], string)> KeyFromPassword(string password, int id, string PasswordSalt = null)
        {
            var User = await _context.Users.FindAsync(id);

            if (User == null)
                return (null, null);

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