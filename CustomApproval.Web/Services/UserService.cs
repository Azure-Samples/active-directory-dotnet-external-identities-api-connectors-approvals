using CustomApproval.Web.Entities;
using CustomApproval.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataRepository _userDataRepository;

        public UserService(IUserDataRepository userDataRepository)
        {
            _userDataRepository = userDataRepository ?? throw new ArgumentNullException(nameof(userDataRepository));
        }

        public async Task CreateUserAsync(UserData userData)
        {
            await _userDataRepository.AddAsync(userData);
        }

        public async Task<bool> IsUserExistAsync(string email, List<string> issuers)
        {
            var existingUser = await _userDataRepository.GetByEmailAndIdentityIssuers(email, issuers);

            return existingUser != null;
        }

        public async Task UpdateUserStatusAsync(string id, string status)
        {
            var existingUser = await _userDataRepository.GetAsync(id);

            if (existingUser == null)
            {
                throw new Exception("User Not Found");

            }

            existingUser.ApprovalStatus = status;

            await _userDataRepository.UpdateAsync(existingUser);
        }

        public async Task<UserData> GetUserByEmailAsync(string email)
        {
            var existingUser = await _userDataRepository.GetByEmail(email);

            return existingUser;
        }

        public async Task<UserData> GetUserByEmailAndIdentityIssuersAsync(string email, List<string> issuers)
        {
             var existingUser = await _userDataRepository.GetByEmailAndIdentityIssuers(email, issuers);

            return existingUser;
        }

        public async Task<IEnumerable<UserData>> GetUsersByEmail(string email)
        {
            return await _userDataRepository.GetListByEmail(email);
        }

        public async Task<UserData> GetUsersById(string id)
        {
            return await _userDataRepository.GetAsync(id);
        }

        public async Task RemoveUsersById(string id)
        {
            await _userDataRepository.RemoveAsync(id);
        }

    }
}
