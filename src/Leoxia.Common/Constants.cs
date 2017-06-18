#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Leoxia Ltd">
//    Copyright (c) 2017 Leoxia Ltd
// </copyright>
// 
// .NET Software Development
// https://www.leoxia.com
// Build. Tomorrow. Together
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//  --------------------------------------------------------------------------------------------------------------------

#endregion

#region Usings

using System;

#endregion

namespace Leoxia.Common
{
    /// <summary>
    ///     Constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     The company name
        /// </summary>
        public const string CompanyName = "Leoxia Ltd";

        /// <summary>
        ///     The vs build tool set2017
        /// </summary>
        public const string VsBuildToolSet2017 =
            @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\";

        /// <summary>
        ///     The ms build VS2017
        /// </summary>
        public const string MsBuildVs2017 =
            "\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Professional\\MSBuild\\15.0\\Bin\\MsBuild.exe\"";

        /// <summary>
        ///     The package directory
        /// </summary>
        public static readonly string PackageDirectory = "//" + Environment.MachineName + "/Packages";

        /// <summary>
        ///     Gets the copyright.
        /// </summary>
        /// <value>
        ///     The copyright.
        /// </value>
        public static string Copyright => "Copyright (c) Leoxia Ltd 2011 - " + DateTime.Now.Year +
                                          ". All Rights reserved.";
    }
}