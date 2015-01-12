//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IReporter.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Pdf
{
    using System;
    using System.IO;

    public interface IReporter: IDisposable
    {
        void Download(string fileName);

        byte[] GetArray();

        void GetStream(Stream stream);
    }
}