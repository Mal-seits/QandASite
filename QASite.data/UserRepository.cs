using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QASite.data
{
    public class UserRepository 
    {

        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public User AuthorizeUser(string email, string password)
        {
            using var context = new QADbContext(_connectionString);
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHashed);
            return isValid ? user : null;
        }
        public void AddUser(string email, string password, string name)
        {
            var passwordHashed = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Email = email,
                Likes = new List<Likes>(),
                Name = name,
                PasswordHashed = passwordHashed
            };
            using var context = new QADbContext(_connectionString);
            context.Users.Add(user);
            context.SaveChanges();
        }
        public User GetUser(string email)
        {
            using var context = new QADbContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
