﻿using BlogApp.Server.Data;
using BlogApp.Server.Models;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Server.Services
{
    public class UsersService
    {
        private readonly DataContext _dataContext;
        public UsersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public UserModel Create (UserModel userModel)
        {
            var newUser = new User
            {
                Name = userModel.Name,
                Email = userModel.Email,
                Password = userModel.Password,
                Description = userModel.Description,
                Photo = userModel.Photo
            };
            _dataContext.Users.Add(newUser);
            _dataContext.SaveChangesAsync();
            userModel.Id = newUser.Id;
            return userModel;
        }
        public UserModel Update(User userToUpdate, UserModel userModel)
        {
            userToUpdate.Name = userModel.Name;
            userToUpdate.Email = userModel.Email;
            userToUpdate.Password = userModel.Password;
            userToUpdate.Description = userModel.Description;
            userToUpdate.Photo = userModel.Photo;

            _dataContext.Users.Update(userToUpdate);
            _dataContext.SaveChangesAsync();

            return userModel;
        }
        public (string login,  string password) GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            string userName = string.Empty;
            string userPass = string.Empty;
            string authHeader = request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePass = authHeader.Replace("Basic ", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");
                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');
                userName = namePassArray[0];
                userPass = namePassArray[1];
            }
            return (userName, userPass);
        }
        public (ClaimsIdentity identity, int id)? GetIdentity(string email, string password)
        {
            User? currentUser = GetUserByLogin(email);
            if (currentUser == null || !VerifyHashedPassword(currentUser.Password, password))
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity
                (
                    claims,
                    "Token",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType
                );
            return (claimsIdentity, currentUser.Id);
        }

        public User? GetUserByLogin(string email)
        {
            return _dataContext.Users.FirstOrDefault(u => u.Email == email);
        }
        public void DeleteUser(User user)
        {
            _dataContext.Users.Remove(user);
            _dataContext.SaveChangesAsync();
        }
        public void Subscribe(int from, int to)
        {
            var sub = new UserSub
            {
                From = from,
                To = to,
                Date = DateTime.Now
            };
            _dataContext.UserSubs.Add(sub);
            _dataContext.SaveChangesAsync();
        }
        private bool VerifyHashedPassword(string password1, string password2)
        {
            return password1 == password2;
        }
    }
}
