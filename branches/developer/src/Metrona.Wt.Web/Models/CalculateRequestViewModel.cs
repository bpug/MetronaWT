//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CalculateRequestModel.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    using Metrona.Wt.Model.Enums;

    public class CalculateRequestViewModel
    {
        public CalculateRequestViewModel()
        {
            BundeslandId = 1;
            //RequestType = RequestType.Plz;
        }
        public int? BundeslandId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",  ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Geben Sie bitte einen Stichtag ein.")]
        //[RegularExpression(@"^(0[1-9]|1[012])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20)\d\d$", ErrorMessage = "Überprüfen Sie bitte den Stichtag.")]
        public DateTime Date { get; set; }

        public int? Plz { get; set; }

        public RequestType RequestType { get; set; }
    }
}