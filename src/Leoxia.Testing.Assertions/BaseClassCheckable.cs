#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseClassCheckable.cs" company="Leoxia Ltd">
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
    ///     Base class for <see cref="ICheckable{T}" /> related to classes.
    /// </summary>
    /// <typeparam name="T">type of the tested value.</typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.BaseCheckable{T}" />
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.IClassCheckable{T}" />
    public abstract class BaseClassCheckable<T> : BaseCheckable<T>, IClassCheckable<T> where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        protected BaseClassCheckable(IExceptionFactory factory, T value) : base(factory, value)
        {
        }

        /// <summary>
        ///     Checks that the tested value is not null.
        /// </summary>
        /// <param name="message">The message.</param>
        public void IsNotNull(string message = null)
        {
            CommonCheck(_value == null, CheckType.NotNull, message);
        }

        /// <summary>
        ///     Checks that the tested value is null.
        /// </summary>
        /// <param name="message">The message.</param>
        public void IsNull(string message = null)
        {
            CommonCheck(_value != null, CheckType.Null, message);
        }

        /// <summary>
        ///     Check that the tested value has properties that respect a contract.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public IPropertiesCheckable<T> HavePropertiesThat(PropertiesComparisonOptions options = null)
        {
            if (options == null)
            {
                options = PropertiesComparisonOptions.Default;
            }
            return new PropertiesCheckable<T>(_factory, _value, options);
        }

        private void CommonCheck(bool flag, CheckType checkType, string message)
        {
            if (flag)
            {
                var classCheckFailure = new ClassCheckFailure<T>(checkType, _value, null, message);
                // ReSharper disable once UnthrowableException
                throw _factory.Build(classCheckFailure);
            }
        }
    }
}