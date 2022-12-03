using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Utils
{
    public static  class PathExtention
    {

        #region default images
        public static string DefaultAvatar = "/img/default/avatar.png";
        #endregion

        #region uploader

        public static string UploadImage = "/img/upload/";
        public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/upload/");

        #endregion

        #region user Avatar
        public static string UserAvatarOrigin = "/Content/Images/UserAvatar/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/origin/");

        public static string UserAvatarThumb = "/Content/Images/UserAvatar/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Thumb/");

        #endregion

        #region slider
        public static string SliderOrigin = "/img/slider/";
        #endregion

        #region siteBanner
        public static string SiteBanner = "/img/bg/";
        #endregion
    }
}
