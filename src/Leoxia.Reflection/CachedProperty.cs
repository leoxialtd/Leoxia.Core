#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachedProperty.cs" company="Leoxia Ltd">
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
using System.Reflection;

#endregion

namespace Leoxia.Reflection
{
    /// <summary>
    ///     Holds type data related to a property
    /// </summary>
    public class CachedProperty
    {
        private readonly Type _elementType;
        private IGetterMethod _getter;
        private ISetterMethod _setter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CachedProperty" /> class.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        public CachedProperty(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            PropertyType = propertyInfo.PropertyType;
            PropertyTypeInfo = PropertyType.GetTypeInfo();
            if (!IsKey)
            {
                IsValidEntityType = !PropertyTypeInfo.IsPrimitive &&
                                    PropertyType != typeof(string) &&
                                    !PropertyTypeInfo.IsEnum;
            }
            _setter = new NotCompiledSetter(propertyInfo.SetMethod);
            _getter = new NotCompiledGetter(propertyInfo.GetMethod);
            IsVirtual = propertyInfo.GetMethod.IsVirtual;
            IsCollectionType = PropertyType.TryGetCollectionElement(out _elementType);
        }

        /// <summary>
        ///     Gets the type of the element. Null if the type is not a collection type.
        /// </summary>
        /// <value>
        ///     The type of the element.
        /// </value>
        public Type ElementType => _elementType;

        /// <summary>
        ///     Gets a value indicating whether this instance is collection type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is collection type; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollectionType { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is key.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is key; otherwise, <c>false</c>.
        /// </value>
        public bool IsKey { get; } = false;

        /// <summary>
        ///     Gets a value indicating whether this instance is valid entity type.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid entity type; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidEntityType { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is virtual.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is virtual; otherwise, <c>false</c>.
        /// </value>
        public bool IsVirtual { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name => PropertyInfo.Name;

        /// <summary>
        ///     Gets the property information.
        /// </summary>
        /// <value>
        ///     The property information.
        /// </value>
        public PropertyInfo PropertyInfo { get; }


        /// <summary>
        ///     Gets the type of the property.
        /// </summary>
        /// <value>
        ///     The type of the property.
        /// </value>
        public Type PropertyType { get; }

        /// <summary>
        ///     Gets the property type information.
        /// </summary>
        /// <value>
        ///     The property type information.
        /// </value>
        public TypeInfo PropertyTypeInfo { get; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <returns></returns>
        public object GetValue(object newEntity)
        {
            return _getter.Invoke(newEntity);
        }

        /// <summary>
        ///     Sets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        public void SetValue(object instance, object value)
        {
            _setter.Invoke(instance, value);
        }

        /// <summary>
        ///     Compiles the getter and setter accessor.
        /// </summary>
        public void Compile()
        {
            _getter = _getter.Compile();
            _setter = _setter.Compile();
        }
    }
}