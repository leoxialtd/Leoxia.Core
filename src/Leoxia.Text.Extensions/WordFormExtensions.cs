#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WordFormExtensions.cs" company="Leoxia Ltd">
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
    ///     Handle singular, plural form of word.
    ///     Depends on language. This is for English.
    /// </summary>
    public static class WordFormExtensions
    {
        /// <summary>
        ///     Return the plural of the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>the plural form of the word</returns>
        public static string Plural(this string word)
        {
            var last = word[word.Length - 1];
            if (last == 'y')
            {
                return word.Substring(0, word.Length - 1) + "ies";
            }
            if (last == 'Y')
            {
                return word.Substring(0, word.Length - 1) + "IES";
            }
            if (last == 's')
            {
                return word + "es";
            }
            if (last == 'S')
            {
                return word + "ES";
            }
            return word + "s";
        }
    }
}