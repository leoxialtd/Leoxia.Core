#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyConfigurationAttribute.cs" company="Leoxia Ltd">
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
    ///     Provides a way to define the behavior of <see cref="ConfigurationReader" />
    ///     when reading the property decorated with this attribute.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyConfigurationAttribute : Attribute
    {
        internal PropertyConfigurationAttribute()
        {
            IsMandatory = true;
            AcceptInvalid = false;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyConfigurationAttribute" /> class.
        /// </summary>
        /// <param name="accepted">The accepted.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">accepted - null</exception>
        public PropertyConfigurationAttribute(ConfigurationAccepted accepted, object defaultValue)
        {
            DefaultValue = defaultValue;
            switch (accepted)
            {
                case ConfigurationAccepted.AcceptMissing:
                    IsMandatory = false;
                    AcceptInvalid = false;
                    break;
                case ConfigurationAccepted.AcceptInvalid:
                    IsMandatory = true;
                    AcceptInvalid = true;
                    break;
                case ConfigurationAccepted.AcceptMissingAndInvalid:
                    IsMandatory = false;
                    AcceptInvalid = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(accepted), accepted, null);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether [accept invalid].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [accept invalid]; otherwise, <c>false</c>.
        /// </value>
        public bool AcceptInvalid { get; }

        /// <summary>
        ///     Gets the default value.
        /// </summary>
        /// <value>
        ///     The default value.
        /// </value>
        public object DefaultValue { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is mandatory.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is mandatory; otherwise, <c>false</c>.
        /// </value>
        public bool IsMandatory { get; }
    }
}