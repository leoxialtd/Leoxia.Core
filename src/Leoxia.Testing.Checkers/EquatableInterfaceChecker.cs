#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquatableInterfaceChecker.cs" company="Leoxia Ltd">
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
using Leoxia.Testing.Assertions;
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Checkers
{
    /// <summary>
    ///     Checker for class implementing IEquatable
    /// </summary>
    public class EquatableInterfaceChecker<TInstance> : IInterfaceChecker where TInstance : IEquatable<TInstance>
    {
        /// <summary>
        ///     Checks assertions depending on the concrete type of checker.
        /// </summary>
        public void CheckInterface()
        {
            var type = typeof(TInstance);
            var result = (TInstance) ObjectBuilder.CreateInstance(type, Environment.TickCount, false);
            ObjectFiller.Fill(result, false);
            Check.That(result.Equals(result)).IsTrue();
            Check.That(result).IsOperatorEqualTo(result);
            Check.That(result).Is(e => e.IsOperatorEqual(result)).IsTrue();
            Check.That(result).Is(e => e.IsOperatorNotEqual(result)).IsFalse();

            Check.That(result.Equals((object) result)).IsTrue();

            Check.That(result.Equals(null)).IsFalse();

            Check.That(result.Equals("")).IsFalse();

            Check.That(result.Equals((object) default(TInstance))).IsFalse();

            Check.That(result.Equals(default(TInstance))).IsFalse();
            Check.That(result).Is(e => e.IsOperatorEqual(default(TInstance))).IsFalse();
            Check.That(result).Is(e => e.IsOperatorNotEqual(default(TInstance))).IsTrue();

            Check.That(result.GetHashCode()).IsEqualTo(result.GetHashCode());

            var clone = (TInstance) ObjectBuilder.MemberwiseClone(result);

            Check.That(result.Equals(clone)).IsTrue();
            Check.That(result).Is(e => e.IsOperatorEqual(clone)).IsTrue();
            Check.That(result).Is(e => e.IsOperatorNotEqual(clone)).IsFalse();

            Check.That(result.Equals((object) clone)).IsTrue();

            if (ObjectModifier.ChangeFirstProperty(clone))
            {
                Check.That(result.Equals(clone)).IsFalse();
                Check.That(result).Is(e => e.IsOperatorEqual(clone)).IsFalse();
                Check.That(result).Is(e => e.IsOperatorNotEqual(clone)).IsTrue();
                Check.That(result.GetHashCode()).IsNotEqualTo(clone.GetHashCode());
            }
        }
    }
}