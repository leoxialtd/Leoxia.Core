#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileInfoExtensions.cs" company="Leoxia Ltd">
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

using Leoxia.Abstractions.IO;
using Newtonsoft.Json.Linq;

#endregion

namespace Leoxia.Serialization.Json
{
    /// <summary>
    ///     Extensions for serialization of <see cref="IFileInfo" />
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        ///     Deserialize the file to T instance.
        /// </summary>
        /// <typeparam name="T">type of object to deserialize</typeparam>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        public static T DeserializeTo<T>(this IFileInfo info)
        {
            return JsonFileSerializer.Deserialize<T>(info);
        }

        /// <summary>
        ///     Deserialize to <see cref="JObject" />
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>the read <see cref="JObject" /></returns>
        public static JObject DeserializeToJObject(this IFileInfo info)
        {
            return JsonFileSerializer.DeserializeJObject(info);
        }
    }
}