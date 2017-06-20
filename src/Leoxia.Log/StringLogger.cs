#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringLogger.cs" company="Leoxia Ltd">
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

using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace Leoxia.Log
{
    /// <summary>
    /// 
    /// </summary>
    public class StringLogger
    {
        /// <summary>
        ///     static constructor
        /// </summary>
        static StringLogger()
        {
            Reset();
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public static List<string> Log { get; private set; }

        /// <summary>
        ///     Reset the logger
        /// </summary>
        public static void Reset()
        {
            Log = new List<string>();
        }

        /// <summary>
        ///     The implementation of write line
        /// </summary>
        internal static void InternalWriteLine(object instance, string name, object[] ps, string prefix)
        {
            var stringBuilder = new StringBuilder();

            //get function name and class name
            stringBuilder.Append(string.Format("{0} {1}.{2}(", prefix,
                instance != null ? instance.GetType().FullName : "static", name));
            var first = true;
            //add parameters
            foreach (var p in ps)
            {
                stringBuilder.Append(p != null ? p.ToString() : "NULL");
                if (first)
                {
                    first = false;
                }
                else
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append(")");

            Log.Add(stringBuilder.ToString());
        }
    }

    /// <summary>
    ///     Simple logger class.
    /// </summary>
    public class StringLogger<T> : StringLogger
    {
        /// <summary>
        ///     Simple logging
        /// </summary>
        public static void LogMethodBefore(T instance, string name, object[] ps)
        {
            InternalWriteLine(instance, name, ps, "Before");
        }

        /// <summary>
        ///     Simple logging
        /// </summary>
        public static void LogMethodAfter(T instance, string name, object[] ps)
        {
            InternalWriteLine(instance, name, ps, "After");
        }

        /// <summary>
        /// Logs the get property after.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="returnValue">The return value.</param>
        /// <returns></returns>
        public static K LogGetPropertyAfter<K>(T instance, string propertyName, K returnValue)
        {
            InternalWriteLine(instance, propertyName, new object[] { }, "After");
            return returnValue;
        }

        /// <summary>
        /// Logs the get property before.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        public static void LogGetPropertyBefore(T instance, string propertyName)
        {
            InternalWriteLine(instance, propertyName, new object[] { }, "Before");
        }

        /// <summary>
        /// Logs the set property before.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static K LogSetPropertyBefore<K>(T instance, string propertyName, K oldValue, K value)
        {
            InternalWriteLine(instance, propertyName, new object[] {value}, "Before");
            return value;
        }

        /// <summary>
        /// Logs the set property after.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The new value.</param>
        public static void LogSetPropertyAfter<K>(T instance, string propertyName, K oldValue, K value, K newValue)
        {
            InternalWriteLine(instance, propertyName, new object[] {value}, "After");
        }
    }
}