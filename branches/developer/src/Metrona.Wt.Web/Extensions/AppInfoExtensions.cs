// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppInfoExtensions.cs" company="ip-connect GmbH">
//   Copyright (c) ip-connect GmbH. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.Extensions
{
    public static class AppInfoExtensions
    {
        public static string GetApplicationSemanticVersion()
        {
            return CommonAssemblyVersion.AssemblyInformationalVersion;
        }

        public static string GetApplicationVersion(bool usePrefix = false)
        {
            return usePrefix
                       ? string.Format("v{0}", CommonAssemblyVersion.AssemblyFileVersion)
                       : CommonAssemblyVersion.AssemblyFileVersion;
        }
    }
}