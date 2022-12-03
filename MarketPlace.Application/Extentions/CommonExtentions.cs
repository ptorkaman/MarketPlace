﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Extentions
{
    public static class CommonExtentions
    {
        public static string GetEnumName(this System.Enum myEnum)
        {
            var enumDisplayName = myEnum.GetType().GetMember(myEnum.ToString()).FirstOrDefault();
            if (enumDisplayName != null)
                return enumDisplayName.GetCustomAttribute<DisplayAttribute>()?.GetName();

            return "";
        }
    }
}
