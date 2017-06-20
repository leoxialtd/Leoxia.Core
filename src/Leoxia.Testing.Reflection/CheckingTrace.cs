#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckingTrace.cs" company="Leoxia Ltd">
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
using System.Collections.Generic;
using System.Text;

#endregion

namespace Leoxia.Testing.Reflection
{
    /// <summary>
    ///     Check trace.
    /// </summary>
    public class CheckingTrace
    {
        private readonly List<string> _frames = new List<string>();
        private string _failureCause;

        /// <summary>
        ///     Sets the failure.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SetFailure(string message)
        {
            _failureCause = message;
        }

        /// <summary>
        ///     Pushes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void Push(Type type)
        {
            _frames.Add("On " + type + ":");
        }

        /// <summary>
        ///     Pushes the index.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="index">The index.</param>
        public void PushIndex(Type type, int index)
        {
            _frames.Add($"On {type}[{index}]: ");
        }

        /// <summary>
        ///     Pushes the property.
        /// </summary>
        /// <param name="containerType">Type of the container.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        public void PushProperty(Type containerType, Type propertyType, string propertyName)
        {
            _frames.Add($"On {containerType}.{propertyName} of type {propertyType}: ");
        }

        /// <summary>
        ///     Pops this instance.
        /// </summary>
        public void Pop()
        {
            _frames.RemoveAt(_frames.Count - 1);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var item in _frames)
            {
                builder.AppendLine(item);
            }
            builder.AppendLine(_failureCause);
            return builder.ToString();
        }
    }
}