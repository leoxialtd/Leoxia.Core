#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrimExtensions.cs" company="Leoxia Ltd">
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

namespace Leoxia.Text.Extensions
{
    /// <summary>
    ///     Extension methods on <see cref="string" /> to trim it.
    /// </summary>
    public static class TrimExtensions
    {
        /// <summary>
        ///     Trim the specified <see cref="string" /> of the start of input <see cref="string" />.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="toTrim"><see cref="string" /> to trim.</param>
        /// <returns>trimmed <see cref="string" /></returns>
        public static string TrimStart(this string input, string toTrim)
        {
            if (input.Length < toTrim.Length)
            {
                return input;
            }
            var shouldTrim = true;
            for (var index = 0; index < toTrim.Length; index++)
            {
                var toTrimChar = toTrim[index];
                if (index >= input.Length)
                {
                    break;
                }
                var inputChar = input[index];
                if (toTrimChar != inputChar)
                {
                    shouldTrim = false;
                    break;
                }
            }
            if (shouldTrim)
            {
                return input.Substring(toTrim.Length, input.Length - toTrim.Length);
            }
            return input;
        }
    }
}