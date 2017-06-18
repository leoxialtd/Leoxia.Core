#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableChecker.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.Checkers
{
    /// <summary>
    ///     Check that a given enumerable respect the enumerable contract
    /// </summary>
    /// <typeparam name="T">type of element</typeparam>
    /// <seealso cref="Leoxia.Testing.Checkers.IInterfaceChecker" />
    public abstract class EnumerableChecker<T> : IInterfaceChecker
    {
        private readonly IEnumerable<T> _enumerable;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnumerableChecker{T}" /> class.
        /// </summary>
        protected EnumerableChecker(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        /// <summary>
        ///     Checks assertions depending on the concrete type of checker.
        /// </summary>
        public void CheckInterface()
        {
            EnumerableInheritorCheck();
            var index = 0;
            foreach (var item in _enumerable)
            {
                CheckElement(index, item);
                index++;
            }
            EnumerableInheritorCheck();
        }

        /// <summary>
        ///     Check the inheritors of <see cref="EnumerableChecker{T}" />
        /// </summary>
        protected abstract void EnumerableInheritorCheck();

        /// <summary>
        ///     Checks the element at the given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected abstract void CheckElement(int index, T item);
    }
}