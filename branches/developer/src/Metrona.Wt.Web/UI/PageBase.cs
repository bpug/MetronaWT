//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PageBase.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.UI
{
    using System;
    using System.Linq;
    using System.Web.UI;

    using Metrona.Wt.Identity.Models;
    using Metrona.Wt.Web.Extensions;

    public class PageBase : Page
    {
        //protected string SubTitle { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            var siteMaster = (SiteMaster)this.Master;
            if (siteMaster != null)
            {
                var title = this.Title.Split(':');
                siteMaster.SiteTitle = title[0];
                //if (title.Count() > 1)
                //    siteMaster.SiteSubTitle = title[1];
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