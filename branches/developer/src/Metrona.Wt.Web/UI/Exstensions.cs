//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Exstensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.UI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;

    using AuthorizeAttribute = Metrona.Wt.Model.Attributes.AuthorizeAttribute;

    public static class Exstensions
    {
        public static SelectList ToSelectList(
            this Enum enumeration,
            object selectedValue = null,
            string defaultItem = null)
        {
            bool isAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;

            var list =
                enumeration.GetType()
                    .GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                    .Select(
                        field => new
                        {
                            Field = field,
                            Authorize =
                                field.GetCustomAttribute(typeof(AuthorizeAttribute), false) as AuthorizeAttribute,
                            Id = field.GetValue(null),
                            //Id = Convert.ChangeType(field.GetValue(null), Enum.GetUnderlyingType(enumeration.GetType())),
                            Display = field.GetCustomAttribute(typeof(DisplayAttribute), false) as DisplayAttribute
                        })
                    .Where(p => CheckAuth(p.Authorize, isAuthenticated))
                    .Select(
                        p => new SelectListQueryItem<object>
                        {
                            Id = p.Id,
                            Name = p.Display != null ? p.Display.Name : p.Field.Name
                        }).ToList();

            if (!string.IsNullOrEmpty(defaultItem))
            {
                list.Insert(
                    0,
                    new SelectListQueryItem<object>
                    {
                        Id = null,
                        Name = defaultItem
                    });
            }
            return new SelectList(list, "Id", "Name", selectedValue);
        }

        private static bool CheckAuth(AuthorizeAttribute attribute, bool isAuthenticated)
        {
            if (attribute != null && !isAuthenticated)
            {
                return false;
            }
            return true;
        }

        internal class SelectListQueryItem<T>
        {
            public T Id { get; set; }

            public string Name { get; set; }
        }

        //public static string GetDescription(this Enum enumeration)
        //{
        //    var type = enumeration.GetType();

        //    var memberInfo = type.GetMember(enumeration.ToString());

        //    if (memberInfo.Length > 0)
        //    {
        //        var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        //        if (attributes.Length > 0)
        //        {
        //            return ((DescriptionAttribute)attributes.First()).Description;
        //        }
        //    }
        //    return enumeration.ToString();
        //    ;
        //}
    }
}