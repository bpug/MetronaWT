//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AuthorizeAttribute.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Attributes
{
    using System;
    using System.Linq;

    public class AuthorizeAttribute : Attribute
    {
        private string roles;

        private string[] rolesSplit;

        public string Roles
        {
            get
            {
                return this.roles ?? string.Empty;
            }
            set
            {
                this.roles = value;
                this.rolesSplit = SplitString(value);
            }
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new string[0];
            }
            return (original.Split(',')).Select(
                piece => new
                {
                    piece = piece,
                    trimmed = piece.Trim()
                }).Where(param => !string.IsNullOrEmpty(param.trimmed)).Select(param => param.trimmed).ToArray();
        }
    }
}