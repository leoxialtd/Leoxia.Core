#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileLockManager.cs" company="Leoxia Ltd">
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

using System.Collections.Generic;
using System.IO;
using System.Threading;
using Leoxia.Threading;

namespace Leoxia.IO
{
    /// <summary>
    /// Provide a <see cref="DisposableMutex"/> lock for a file
    /// </summary>
    public class FileLockManager
    {
        private static readonly object _syncRoot = new object();
        private static readonly IDictionary<string, Mutex> _dictionary = new Dictionary<string, Mutex>();

        /// <summary>
        /// Gets the lock related to a given file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static DisposableMutex GetLock(string filePath)
        {
            Mutex locker;
            lock (_syncRoot)
            {
                if (!_dictionary.TryGetValue(filePath, out locker))
                {
                    locker = new Mutex(false, filePath.Replace(Path.DirectorySeparatorChar, '\f'));
                    _dictionary[filePath] = locker;
                }
            }
            return new DisposableMutex(locker);
        }
    }
}