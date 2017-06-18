﻿#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingMandatoryConfigurationException.cs" company="Leoxia Ltd">
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

namespace Leoxia.Configuration
{
    /// <summary>
    ///     Exception raised when mandatory configuration is missing.
    /// </summary>
    /// <seealso>
    ///     <cref>System.Exception</cref>
    /// </seealso>
    public sealed class MissingMandatoryConfigurationException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingMandatoryConfigurationException" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        public MissingMandatoryConfigurationException(Type type, string property) : base(GetMessage(type, property))
        {
        }

        private static string GetMessage(Type type, string property)
        {
            return $" Missing mandatory {property} in section {type.Name} of IConfiguration";
        }
    }
}