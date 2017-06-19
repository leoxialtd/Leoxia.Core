#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonHelper.cs" company="Leoxia Ltd">
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

using System.IO;
using Leoxia.Abstractions.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace Leoxia.Serialization.Json
{
    /// <summary>
    /// Serialize and deserialize data from <see cref="IFileInfo"/>
    /// </summary>
    /// <seealso cref="IJsonFileSerializer" />
    public class JsonFileSerializer : IJsonFileSerializer
    {
        private readonly IFile _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFileSerializer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public JsonFileSerializer(IFile fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Deserialize a T object from a file located at the specified file path.
        /// </summary>
        /// <typeparam name="T">type of object to deserialize</typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns>
        /// deserialized T
        /// </returns>
        public T Deserialize<T>(string filePath)
        {
            using (var fileReader = _fileSystem.OpenText(filePath))
            {
                return JsonConvert.DeserializeObject<T>(fileReader.ReadToEnd());
            }
        }

        /// <summary>
        /// Deserialize the specified file.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize</typeparam>
        /// <param name="file">The file.</param>
        /// <returns>deserialized T</returns>
        public static T Deserialize<T>(IFileInfo file)
        {
            using (var fileReader = file.OpenText())
            {
                return JsonConvert.DeserializeObject<T>(fileReader.ReadToEnd());
            }
        }

        /// <summary>
        /// Deserialize the <see cref="JObject"/>.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>deserialized <see cref="JObject"/></returns>
        public static JObject DeserializeJObject(IFileInfo file)
        {
            using (var fileReader = file.OpenText())
            {
                using (var reader = new StringReader(fileReader.ReadToEnd()))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        return (JObject) JToken.ReadFrom(jsonReader);
                    }
                }
            }
        }
    }
}