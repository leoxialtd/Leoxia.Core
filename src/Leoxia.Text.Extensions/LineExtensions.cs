#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineExtensions.cs" company="Leoxia Ltd">
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
using System.Collections.Generic;

#endregion

namespace Leoxia.Text.Extensions
{
    /// <summary>
    ///     Lines related methods
    /// </summary>
    public static class LineExtensions
    {
        /// <summary>
        ///     Splits the input in lines.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string[] SplitInLines(this string input)
        {
            var res = input.Replace("\r\n", "\f");
            return res.Split('\f', '\n', '\r');
        }

        /// <summary>
        ///     Joins the input lines in a text where lines are separated by new line character.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <returns></returns>
        public static string JoinLines(this IEnumerable<string> lines)
        {
            return string.Join(Environment.NewLine, lines);
        }
    }
}