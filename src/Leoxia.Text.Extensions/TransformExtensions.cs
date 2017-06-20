#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformExtensions.cs" company="Leoxia Ltd">
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

using System.Linq;

#endregion

namespace Leoxia.Text.Extensions
{
    /// <summary>
    ///     Transform characters in a <see cref="string" />
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        ///     Replace all characters to replace by the replacement character.
        /// </summary>
        /// <param name="input">The input <see cref="string" /></param>
        /// <param name="toReplace">characters to replace.</param>
        /// <param name="replacement">The <see cref="char" /> replacement.</param>
        /// <returns><see cref="string" /> with characters replaced.</returns>
        public static string MultiReplace(this string input, char[] toReplace, char replacement)
        {
            var destination = input.ToCharArray();
            for (var index = 0; index < destination.Length; index++)
            {
                var c = destination[index];
                if (toReplace.Contains(c))
                {
                    destination[index] = replacement;
                }
            }
            return string.Concat(destination);
        }
    }
}