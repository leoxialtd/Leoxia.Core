#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesComparisonOptions.cs" company="Leoxia Ltd">
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

using System.Collections.Generic;

#endregion

namespace Leoxia.Testing.Reflection
{
    /// <summary>
    ///     Options used for comparing properties.
    /// </summary>
    public class PropertiesComparisonOptions
    {
        /// <summary>
        ///     The default singleton options
        /// </summary>
        public static readonly PropertiesComparisonOptions Default = new PropertiesComparisonOptions();

        /// <summary>
        ///     The options that do not check for number of properties
        /// </summary>
        public static readonly PropertiesComparisonOptions DoNotCheckForNumberOfProperties =
            BuildDoNotCheckForNumberOfProperties();

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertiesComparisonOptions" /> class.
        /// </summary>
        public PropertiesComparisonOptions()
        {
            // By default check for number of properties
            CheckForNumberOfProperties = true;
            IgnoreReadOnlyProperties = true;
        }

        /// <summary>
        ///     Gets a value indicating whether [check for number of properties].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [check for number of properties]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckForNumberOfProperties { get; private set; }

        /// <summary>
        ///     Gets or sets the excluded properties.
        /// </summary>
        /// <value>
        ///     The excluded properties.
        /// </value>
        public IEnumerable<string> ExcludedProperties { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [ignore read only properties].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [ignore read only properties]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreReadOnlyProperties { get; set; }

        private static PropertiesComparisonOptions BuildDoNotCheckForNumberOfProperties()
        {
            return new PropertiesComparisonOptions {CheckForNumberOfProperties = false};
        }
    }
}