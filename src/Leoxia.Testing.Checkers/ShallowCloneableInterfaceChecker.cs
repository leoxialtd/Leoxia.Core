#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShallowCloneableInterfaceChecker.cs" company="Leoxia Ltd">
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
using Leoxia.Abstractions;
using Leoxia.Testing.Assertions;
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Checkers
{
    /// <summary>
    ///     Check ICloneable implementation is ok for the given type.
    /// </summary>
    /// <typeparam name="TCloneable">The type of the cloneable.</typeparam>
    public class ShallowCloneableInterfaceChecker<TCloneable> : IInterfaceChecker where TCloneable : IShallowCloneable
    {
        /// <summary>
        ///     Checks assertions depending on the concrete type of checker.
        /// </summary>
        public void CheckInterface()
        {
            var result = (TCloneable) ObjectBuilder.CreateInstance(typeof(TCloneable), Environment.TickCount, false);
            ObjectFiller.Fill(result, false);
            var clone = (TCloneable) result.Clone();
            // Clone should be equal
            Check.ThatObject(result).IsEqualTo(clone);
            // Check if we change a property on a side it is not changed on other side
            foreach (var info in typeof(TCloneable).GetProperties())
            {
                clone = (TCloneable) result.Clone();
                if (ObjectModifier.ChangeValue(result, info.Name, true))
                {
                    var type = result.GetType();
                    Check.ThatObject(info.GetValue(result)).IsNotEqualTo(
                        info.GetValue(clone),
                        $"After clone of {type}, property {info.Name} should not remain equal to the same property of original object");
                }
            }
        }
    }
}