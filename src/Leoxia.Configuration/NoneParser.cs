#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoneParser.cs" company="Leoxia Ltd">
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

namespace Leoxia.Configuration
{
    /// <summary>
    ///     Parser that doesn't make any treatment on the configured value.
    /// </summary>
    /// <seealso cref="Leoxia.Configuration.IParser" />
    internal class NoneParser : IParser
    {
        /// <summary>
        ///     Parses the specified unparsed value.
        /// </summary>
        /// <param name="unparsedValue">The unparsed value.</param>
        /// <returns>parsed value boxed into object</returns>
        public object Parse(string unparsedValue)
        {
            return unparsedValue;
        }

        /// <summary>
        ///     Tries to parse the specified unparsed value or return default value.
        /// </summary>
        /// <param name="unparsedValue">The unparsed value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>parsed value or default value</returns>
        public object TryParse(string unparsedValue, object defaultValue)
        {
            if (unparsedValue == null)
            {
                return defaultValue;
            }
            return unparsedValue;
        }
    }
}