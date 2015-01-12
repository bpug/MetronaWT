//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UserStoreService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Identity.Services
{
    using System;
    using System.Threading.Tasks;

    using Metrona.Wt.Database.Repositories;
    using Metrona.Wt.Identity.Models;

    public class UserStoreService : IUserStoreService
    {
        private readonly IUserRepository userRepository;

        public UserStoreService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<ApplicationUser> FindByIdAsync(long userId)
        {
            var user = await this.userRepository.GetAsync(userId);
            return user.ToApplicationUser();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = await this.userRepository.FindByNameAsync(userName);
            return user.ToApplicationUser();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Password != null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}