//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PageBase.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.UI
{
    using System;
    using System.Web.UI;

    using Metrona.Wt.Identity.Models;
    using Metrona.Wt.Web.Extensions;

    public class PageBase : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            var siteMaster = (SiteMaster)this.Master;
            if (siteMaster != null)
            {
                siteMaster.SiteTitle = this.Title;
            }

            base.OnLoad(e);
        }

        protected ApplicationUser CurrentUser
        {
            get
            {
                return User.ToAppUser();
            }
        }
    }
}