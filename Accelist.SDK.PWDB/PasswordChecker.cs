using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Linq;

namespace Accelist.SDK.PWDB
{
    /// <summary>
    /// Contains methods for validating a secure password.
    /// </summary>
    public static class PasswordChecker
    {
        private static HashSet<string> _ForbiddenPasswords;

        private static object _ForbiddenPasswordsLock = new object();

        /// <summary>
        /// Returns a lazy-loaded hashed collection of most common passwords.
        /// </summary>
        public static HashSet<string> ForbiddenPasswords
        {
            get
            {
                if (_ForbiddenPasswords == null)
                {
                    lock (_ForbiddenPasswordsLock)
                    {
                        if (_ForbiddenPasswords == null)
                        {
                            var passwords = UnzipPasswords(ZippedPasswords);
                            _ForbiddenPasswords = new HashSet<string>(passwords);
                        }
                    }
                }

                return _ForbiddenPasswords;
            }
        }

        /// <summary>
        /// Gets a stream of zipped passwords.
        /// </summary>
        public static Stream ZippedPasswords
        {
            get
            {
                return typeof(PasswordChecker).GetTypeInfo().Assembly.GetManifestResourceStream("Accelist.SDK.PWDB.rockyou3min9.zip");
            }
        }

        /// <summary>
        /// Checks whether password input string is of sufficient length and not blacklisted.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            if (password.Length < 9)
            {
                return false;
            }

            if (ForbiddenPasswords.Contains(password))
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Returns a list of string from a zipped text file that contains passwords separated by newlines.
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static IEnumerable<string> UnzipPasswords(Stream zip)
        {
            using (var archive = new ZipArchive(zip))
            {
                var stream = archive.Entries.First().Open();

                using (var reader = new StreamReader(stream))
                {
                    string s;
                    while ((s = reader.ReadLine()) != null)
                    {
                        yield return s;
                    }
                }
            }
        }
    }
}
