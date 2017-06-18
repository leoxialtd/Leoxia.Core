#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamFactory.cs" company="Leoxia Ltd">
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
using System.IO;
using System.Reflection;

#endregion

namespace Leoxia.Log.IO
{
    internal class FileStreamFactory : IStreamFactory
    {
        public StreamWriter CreateStreamWriter(string logFilePath)
        {
            if (!Path.IsPathRooted(logFilePath))
            {
                var assemblyPath = ApplicationHelper.GetApplicationPath();
                var directory = Path.GetDirectoryName(assemblyPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    logFilePath = Path.Combine(directory, logFilePath);
                }
            }
            return File.CreateText(logFilePath);
        }
    }

    public static class ApplicationHelper
    {
        public static string GetDefautLogDirectory()
        {
            return GetApplicationPath();
        }

        public static string GetApplicationPath()
        {
            try
            {
                return typeof(Assembly).InvokeStaticMethod<Assembly>("GetEntryAssembly")
                    .InvokeProperty<string>("Location");
            }
            catch (Exception)
            {
                // HACK. Not really working. Should be better. Especially on phones. 
                return Directory.GetCurrentDirectory();
            }
        }
    }

    public static class ReflectionHelper
    {
        private static readonly Type[] _emptyTypes = new Type[0];
        private static readonly object[] _emptyParameters = new object[0];

        public static TReturnType InvokeStaticMethod<TReturnType>(this Type type, string methodName)
        {
            var method = type.GetRuntimeMethod(methodName, _emptyTypes);
            return (TReturnType) method.Invoke(null, _emptyParameters);
        }

        public static TReturnType InvokeProperty<TReturnType>(this object instance, string propertyName)
        {
            var type = instance.GetType();
            var property = type.GetRuntimeProperty(propertyName);
            return (TReturnType) property.GetValue(instance);
        }
    }
}