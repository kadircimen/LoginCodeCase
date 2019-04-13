using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginCodeCase.Interfaces;
using LoginCodeCase.Model;
using LoginCodeCase.Helpers;

namespace LoginCodeCase.Services
{
    public class UserService : IUser
    {
        //IUser interface'i implemente edilen soyut sınıfın servisleri
        private DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }
        public User Authenticate(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == email);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.passHash, user.passSalt))
                return null;

            return user;
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetByID(int id)
        {
            return _context.Users.Find(id);
        }
        public User GetByEmail(string mail)
        {
            return _context.Users.FirstOrDefault(x => x.Email == mail);
        }
        public User Create(User user, string password)
        {
            //validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Şifre Gereklidir!");

            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException(user.Email + " - E-Posta adresi daha önce başkası tarafından kaydedilmiş!");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.passHash = passwordHash;
            user.passSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
        public void Update(int UserID, string password = null)
        {
            var user = _context.Users.Find(UserID);

            if (user == null)
                throw new AppException("Kullanıcı Bulunamadı");


            //update user properties
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.passHash = passwordHash;
                user.passSalt = passwordSalt;
            }
            _context.Users.Update(user);
            _context.SaveChanges();

        }
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }

}
