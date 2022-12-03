using MarketPlace.Application.Extensions;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constructor
        private readonly IGenericRepository<User> _userRepositoy;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ISmsService _smsService;
        
        public UserService(IGenericRepository<User> userRepositoy,IPasswordHelper passwordHelper,ISmsService smsService)
        {
            _userRepositoy = userRepositoy;
            _passwordHelper = passwordHelper;
            _smsService = smsService;
        }
        #endregion

        #region account

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            if (!await IsUserExistsMobileNumber(register.Mobile))
            {
                var User = new User()
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Password = _passwordHelper.EncodePasswordMd5(register.Mobile),
                    Mobile = register.Mobile,
                    MobileActiveCode = new Random().Next(10000,99999).ToString(),
                    EmailActiveCode = Guid.NewGuid().ToString("N")
                };
                await _userRepositoy.AddEntity(User);
                await _userRepositoy.SaveChanges();
                await _smsService.SendVerificationSMS(User.Mobile,User.MobileActiveCode);
                
                return RegisterUserResult.Succcess;
            }
            else
            {
                return RegisterUserResult.MobileExists;
            }
            
            return RegisterUserResult.Error;
        }

        public async Task<bool> IsUserExistsMobileNumber(string mobile)
        {
            return await _userRepositoy.GetQuery().AsQueryable().AnyAsync(a => a.Mobile==mobile);
        }

        public async Task<LoginUserResults> GetUserForLogin(LoginUserDTO login)
        {
            var user = await _userRepositoy.GetQuery().AsQueryable().SingleOrDefaultAsync(s => s.Mobile == login.Mobile);
            if (user == null) return LoginUserResults.NotFound;
            if (!user.IsMobileActive) return LoginUserResults.NotActivated;
            if (user.Password != _passwordHelper.EncodePasswordMd5(login.Password)) return LoginUserResults.NotFound;

            return LoginUserResults.Success;
        }

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _userRepositoy.GetQuery().AsQueryable().SingleOrDefaultAsync(a => a.Mobile == mobile);
            
        }

        public async Task<ForgotPasswordResults> RecoverUserPassword(ForgotPasswordDTO forgot)
        {
            var user = await _userRepositoy.GetQuery().AsQueryable().SingleOrDefaultAsync(a => a.Mobile == forgot.Mobile);
            if (user == null) return ForgotPasswordResults.NotFound;
            var newPassword = new Random().Next(1000000, 999999999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newPassword);
            _userRepositoy.UpdateEntity(user);
            await _smsService.SendUserPasswordSMS(user.Mobile, newPassword);
            await _userRepositoy.SaveChanges();
            return ForgotPasswordResults.Success;
        }

        public async Task<bool> ActivateMobile(ActivateMobileDTO activate)
        {
            var user = await _userRepositoy.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(a => a.Mobile == activate.Mobile);
            if(user != null)
            {
                if(user.MobileActiveCode == activate.ActiveCode)
                {
                 user.IsMobileActive = true;
                 user.MobileActiveCode= new Random().Next(1000000, 999999999).ToString();
                    await _userRepositoy.SaveChanges();
                 return true;

                }
            }
            return false;
        }

        public async Task<bool> ChangeUserPassword(ChangePasswordDTO changePassword, long userId)
        {
            var user = await _userRepositoy.GetEntityById(userId);
            if(user != null)
            {
                var newPassword = _passwordHelper.EncodePasswordMd5(changePassword.NewPassword);
                if ( newPassword != user.Password)
                {
                    user.Password = newPassword;
                    _userRepositoy.UpdateEntity(user);
                    await _userRepositoy.SaveChanges();
                    return true;
                }
                    
            }
            return false;
        }


        public async Task<EditUserProfileDTO> GetUserProfile(long userid)
        {
            var user = await _userRepositoy.GetEntityById(userid);
            if (user == null) return null;

            EditUserProfileDTO userprofile= new EditUserProfileDTO() { 
                FirstName=user.FirstName,
                LastName=user.LastName,
                Avatar=user.Avatar
            };

            return userprofile;

        }


        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId, IFormFile avatarImage)
        {
            var user = await _userRepositoy.GetEntityById(userId);
            if (user.IsBlocked) return EditUserProfileResult.IsBlocked;
            if (!user.IsMobileActive) return EditUserProfileResult.IsNotActive;
            if (user == null) return EditUserProfileResult.NotFound;

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;

            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName, PathExtention.UserAvatarOriginServer, 100, 100, PathExtention.UserAvatarThumbServer, user.Avatar);
                user.Avatar = imageName;
            }
            _userRepositoy.UpdateEntity(user);
            await _userRepositoy.SaveChanges();
            return EditUserProfileResult.Success;
        }

        #endregion


        #region dispose
        public async ValueTask DisposeAsync()
        {
            await _userRepositoy.DisposeAsync();
        }


        #endregion

    }
}
