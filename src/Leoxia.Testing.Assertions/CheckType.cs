#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckType.cs" company="Leoxia Ltd">
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

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    ///     The different check types.
    /// </summary>
    public enum CheckType
    {
        /// <summary>
        ///     The true
        /// </summary>
        True,

        /// <summary>
        ///     The false
        /// </summary>
        False,

        /// <summary>
        ///     The null
        /// </summary>
        Null,

        /// <summary>
        ///     The not null
        /// </summary>
        NotNull,

        /// <summary>
        ///     The equal
        /// </summary>
        Equal,

        /// <summary>
        ///     The not equal
        /// </summary>
        NotEqual,

        /// <summary>
        ///     The op equal
        /// </summary>
        OpEqual,

        /// <summary>
        ///     The op not equal
        /// </summary>
        OpNotEqual,

        /// <summary>
        ///     The count not equal
        /// </summary>
        CountNotEqual,

        /// <summary>
        ///     The list item not equal
        /// </summary>
        ListItemNotEqual,

        /// <summary>
        ///     The list item all equal
        /// </summary>
        ListItemAllEqual,

        /// <summary>
        ///     The string null or empty
        /// </summary>
        StringNullOrEmpty,

        /// <summary>
        ///     The string not null or empty
        /// </summary>
        StringNotNullOrEmpty,

        /// <summary>
        ///     The properties not equal
        /// </summary>
        PropertiesNotEqual,

        /// <summary>
        ///     The properties not initialized
        /// </summary>
        PropertiesNotInitialized,

        /// <summary>
        ///     The properties equal
        /// </summary>
        PropertiesEqual,

        /// <summary>
        ///     The properties initialized
        /// </summary>
        PropertiesInitialized,

        /// <summary>
        ///     The count equal
        /// </summary>
        CountEqual,

        /// <summary>
        ///     The list item equal
        /// </summary>
        ListItemEqual,

        /// <summary>
        ///     The list item is not contained
        /// </summary>
        ListItemIsNotContained
    }
}