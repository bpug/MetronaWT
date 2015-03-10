//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CalculateRequestModel.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Metrona.Wt.Model.Enums;

    public class CalculateRequestViewModel
    {
       public int? BundeslandId { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",  ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        //[RegularExpression(@"^(0[1-9]|1[012])[-/.](0[1-9]|[12][0-9]|3[01])[-/.](19|20)\d\d$", ErrorMessage = "Überprüfen Sie bitte den Stichtag.")]
        [Required(ErrorMessage = "Bitte treffen Sie beim Abrechnungszeitraum Ihre Auswahl")]
        public DateTime? Date { get; set; }

        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Postleitzahl muss 5-stellig sein.")]
        public int? Plz { get; set; }

        [Required(ErrorMessage = "Bitte treffen Sie bei der Region Ihre Auswahl")]
        public RequestType? RequestType { get; set; }
    }
}