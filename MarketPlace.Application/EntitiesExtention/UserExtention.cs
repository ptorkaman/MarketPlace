using MarketPlace.DataLayer.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.EntitiesExtention
{
    public static class UserExtention
    {
        public static string GetUserShowName(this User user)
        {
            if(!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}";
            }
            return user.Mobile;
        }
    }
}
