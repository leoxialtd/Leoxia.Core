#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompileSetter.cs" company="Leoxia Ltd">
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
using System.Linq;
using System.Reflection;

#endregion

namespace Leoxia.Reflection
{
    /// <summary>
    ///     Holds a setter and provides a compiled invocation.
    /// </summary>
    /// <seealso cref="Leoxia.Reflection.ISetterMethod" />
    public class CompileSetter : ISetterMethod
    {
        private readonly Delegate _delegate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompileSetter" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        public CompileSetter(MethodInfo method)
        {
            var type = typeof(Action<,>).MakeGenericType(method.DeclaringType,
                method.GetParameters().FirstOrDefault().ParameterType);
            _delegate = method.CreateDelegate(type);
        }

        /// <summary>
        ///     Invokes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public void Invoke(object instance, object value)
        {
            _delegate.DynamicInvoke(instance, value);
        }

        /// <summary>
        ///     Compiles this instance.
        /// </summary>
        /// <returns></returns>
        public ISetterMethod Compile()
        {
            return this;
        }
    }
}