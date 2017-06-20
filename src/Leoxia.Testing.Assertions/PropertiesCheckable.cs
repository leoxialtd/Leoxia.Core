#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesCheckable.cs" company="Leoxia Ltd">
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

using Leoxia.Testing.Assertions.Abstractions;
using Leoxia.Testing.Assertions.Failures;
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    ///     Checks of properties
    /// </summary>
    /// <typeparam name="T">type of properties container</typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.IPropertiesCheckable{T}" />
    public class PropertiesCheckable<T> : IPropertiesCheckable<T>
    {
        private readonly IExceptionFactory _factory;
        private readonly PropertiesComparisonOptions _options;
        private readonly T _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertiesCheckable{T}" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        public PropertiesCheckable(IExceptionFactory factory, T value, PropertiesComparisonOptions options)
        {
            _factory = factory;
            _value = value;
            _options = options;
        }

        /// <summary>
        ///     Check that the properties are equal to properties of another type
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="PropertiesCheckFailure{T}"></exception>
        public void AreEqualToPropertiesOf(T expected, string message = null)
        {
            var trace = new CheckingTrace();
            if (!ObjectComparer.PropertiesAreEqual(_value, expected, trace, _options))
            {
                // ReSharper disable once UnthrowableException
                throw _factory.Build(new PropertiesCheckFailure<T>(CheckType.PropertiesEqual, _value, expected, trace,
                    message));
            }
        }

        /// <summary>
        ///     Check that the properties are initialized.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="PropertiesCheckFailure{T}"></exception>
        public void AreInitialized(string message = null)
        {
            var trace = new CheckingTrace();
            if (!ObjectTester.PropertiesAreInitialized(_value, trace, _options))
            {
                // ReSharper disable once UnthrowableException
                throw _factory.Build(new PropertiesCheckFailure<T>(CheckType.PropertiesInitialized, _value, default(T),
                    trace, message));
            }
        }
    }
}