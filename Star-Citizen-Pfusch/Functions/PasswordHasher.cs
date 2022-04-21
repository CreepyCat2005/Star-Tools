using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Functions
{
    class PasswordHasher
    {
        public string saltString;
        public PasswordHasher()
        {
        }

        public string hashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            RandomNumberGenerator.Create().GetNonZeroBytes(salt);

            saltString = Convert.ToBase64String(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
        public string hashPassword(string password, string salt)
        {
            byte[] saltArr = Convert.FromBase64String(salt);

            saltString = salt;

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltArr,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
