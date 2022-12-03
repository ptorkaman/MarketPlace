using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.DataLayer.Entities.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        #region account

        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);

        Task<bool> IsUserExistsMobileNumber(string mobile);

        Task<LoginUserResults> GetUserForLogin(LoginUserDTO login);

        Task<User> GetUserByMobile(string mobile);

        Task<ForgotPasswordResults> RecoverUserPassword(ForgotPasswordDTO forgot);

        Task<bool> ActivateMobile(ActivateMobileDTO activate);

        Task<bool> ChangeUserPassword(ChangePasswordDTO changePassword, long userId);

        Task<EditUserProfileDTO> GetUserProfile(long userid);

        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId, IFormFile avatarImage);
        #endregion
    }
}
