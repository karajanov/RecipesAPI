﻿using CookbookProject.Models;
using System.Threading.Tasks;

namespace CookbookProject.Services.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsEmailTakenAsync(string email);

        Task<bool> IsUsernameTakenAsync(string username);

        Task<bool> IsUserValidAsync(string username, string password);
    }
}
