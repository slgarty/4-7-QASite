using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace QASite407_2_.Data
{
    public class QARepository
    {
        private readonly string _connectionString;
        public QARepository(string connectionString)
        {
            connectionString = _connectionString;
        }
        public void AddUser(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            using (var context = new QADbContext(_connectionString))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public User GetByEmail(string email)
        {
            using (var context = new QADbContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;

        }
    }
}
