#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoolCheckable.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    ///     Checks for <see cref="bool" />
    /// </summary>
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.IBoolCheckable" />
    public class BoolCheckable : IBoolCheckable
    {
        private readonly IExceptionFactory _factory;
        private readonly bool _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BoolCheckable" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public BoolCheckable(IExceptionFactory factory, bool value)
        {
            _factory = factory;
            _value = value;
        }

        /// <summary>
        ///     Checks the current value is true.
        /// </summary>
        /// <param name="message">The message.</param>
        public void IsTrue(string message = null)
        {
            Check(!_value, CheckType.True, message);
        }

        /// <summary>
        ///     Checks the current value is false.
        /// </summary>
        /// <param name="message">The message.</param>
        public void IsFalse(string message = null)
        {
            Check(_value, CheckType.False, message);
        }

        private void Check(bool flag, CheckType checkType, string message)
        {
            if (flag)
            {
                var checkFailure = new BoolCheckFailure(checkType, _value, !_value, message);
                // ReSharper disable once UnthrowableException
                throw _factory.Build(checkFailure);
            }
        }
    }
}