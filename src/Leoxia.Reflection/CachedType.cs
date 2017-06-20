#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachedType.cs" company="Leoxia Ltd">
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Leoxia.Reflection
{
    /// <summary>
    ///     Type information cache.
    /// </summary>
    public class CachedType
    {
        private static readonly ConcurrentDictionary<Type, CachedType> _cachedTypes =
            new ConcurrentDictionary<Type, CachedType>();

        private readonly IDictionary<string, CachedProperty> _cachedProperties;

        private CachedType(Type type)
        {
            Info = type.GetTypeInfo();
            var properties = Info.GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.FlattenHierarchy);
            _cachedProperties = properties.Select(p => new CachedProperty(p)).ToDictionary(x => x.Name);
            HasCollectionProperties = _cachedProperties.Values.Any(x => x.IsCollectionType);
            if (Info.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                var indexOfQuote = genericTypeDefinition.Name.IndexOf('`');
                var genericPart = genericTypeDefinition.Name.Substring(0, indexOfQuote);
                var genericArguments = type.GenericTypeArguments;
                var innerPart = string.Join(", ", genericArguments.Select(x => x.GetValidName()));
                Name = genericPart + "<" + innerPart + ">";
            }
            else
            {
                Name = type.Name;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance has collection properties.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has collection properties; otherwise, <c>false</c>.
        /// </value>
        public bool HasCollectionProperties { get; }

        /// <summary>
        ///     Gets the type information <see cref="TypeInfo" />.
        /// </summary>
        /// <value>
        ///     The information.
        /// </value>
        public TypeInfo Info { get; }

        /// <summary>
        ///     Always provide the valid (compilable) type name.
        ///     Useful for generics.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Gets the cached properties.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CachedProperty> GetProperties()
        {
            return _cachedProperties.Values;
        }

        /// <summary>
        ///     Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static CachedType Get(Type type)
        {
            return _cachedTypes.GetOrAdd(type, CreateCached);
        }

        /// <summary>
        ///     Creates the cached type.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        private static CachedType CreateCached(Type arg)
        {
            return new CachedType(arg);
        }

        /// <summary>
        ///     Determines whether the specified property name has property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     <c>true</c> if the specified property name has property; otherwise, <c>false</c>.
        /// </returns>
        public bool HasProperty(string propertyName)
        {
            return _cachedProperties.ContainsKey(propertyName);
        }

        /// <summary>
        ///     Tries to get the value of the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="destinationCachedProperty">The destination cached property.</param>
        /// <returns></returns>
        public bool TryGetValue(string propertyName, out CachedProperty destinationCachedProperty)
        {
            return _cachedProperties.TryGetValue(propertyName, out destinationCachedProperty);
        }

        /// <summary>
        ///     Compiles the member access.
        /// </summary>
        public void Compile()
        {
            foreach (var property in _cachedProperties.Values)
            {
                property.Compile();
            }
        }
    }
}