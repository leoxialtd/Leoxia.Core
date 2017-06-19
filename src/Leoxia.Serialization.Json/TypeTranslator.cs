#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeTranslator.cs" company="Leoxia Ltd">
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
using Newtonsoft.Json.Linq;

#endregion

namespace Leoxia.Serialization.Json
{
    /// <summary>
    /// Translator of Json Types
    /// </summary>
    public class JsonTypeTranslator
    {
        /// <summary>
        /// Gets the equivalent .NET <see cref="Type"/> for a <see cref="JTokenType"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>translated <see cref="Type"/></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">type - null</exception>
        public static Type GetEquivalentType(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Object:
                    return typeof(object);
                case JTokenType.Integer:
                    return typeof(long);
                case JTokenType.Float:
                    return typeof(decimal);
                case JTokenType.String:
                    return typeof(string);
                case JTokenType.Boolean:
                    return typeof(bool);
                case JTokenType.Date:
                    return typeof(DateTime);
                case JTokenType.Null:
                    return typeof(object);
                case JTokenType.Array: // Only array of string are managed
                    return typeof(string[]);
                case JTokenType.Property:
                case JTokenType.Comment:
                case JTokenType.Undefined:
                case JTokenType.None:
                case JTokenType.Constructor:
                case JTokenType.Raw:
                    throw new NotSupportedException($"property {type} is not supported");
                case JTokenType.Bytes:
                    return typeof(byte[]);
                case JTokenType.Guid:
                    return typeof(Guid);
                case JTokenType.Uri:
                    return typeof(string);
                case JTokenType.TimeSpan:
                    return typeof(TimeSpan);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}