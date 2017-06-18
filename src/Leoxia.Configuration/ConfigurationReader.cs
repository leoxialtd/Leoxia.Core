#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationReader.cs" company="Leoxia Ltd">
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
using Leoxia.Reflection;
using Microsoft.Extensions.Configuration;

#endregion

namespace Leoxia.Configuration
{
    /// <summary>
    ///     Read configuration and populate configuration based on attribute on configuration class.
    /// </summary>
    public class ConfigurationReader
    {
        private readonly IConfiguration _configuration;
        private readonly IParser _noneParser = new NoneParser();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigurationReader" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ConfigurationReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///     Reads this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Read<T>() where T : class, new()
        {
            var instance = new T();
            return Read(instance);
        }

        /// <summary>
        ///     Reads the configuration and populates the given instance (or newly created one) properties according to it.<br />
        ///     By default if the instance type has a property, it is considered as mandatory in configuration and value in
        ///     configuration
        ///     should be valid.<br />
        ///     Use <see cref="PropertyConfigurationAttribute" /> or <see cref="IgnoredPropertyAttribute" />
        ///     to changed that behavior.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="MissingMandatoryConfigurationException"></exception>
        public T Read<T>(T instance) where T : class
        {
            var type = CachedType.Get(typeof(T));
            var sectionName = GetSectionName(type);
            var section = _configuration.GetSection(sectionName);
            if (section == null)
            {
                throw new InvalidOperationException("IConfiguration should never return null section by contract.");
            }
            // If section doesn't exists should throw or at least warn ?
            foreach (var property in type.GetProperties())
            {
                SetPropertyValue(instance, property, section);
            }
            return instance;
        }

        private void SetPropertyValue<T>(T instance, CachedProperty property, IConfigurationSection section)
            where T : class
        {
            if (IsIgnored(property))
            {
                return;
            }
            var propertyConfiguration = GetPropertyConfiguration(property);
            var unparsedValue = section[property.Name];
            var value = GetParsedValue<T>(property, unparsedValue, propertyConfiguration);
            property.SetValue(instance, value);
        }

        private object GetParsedValue<T>(CachedProperty property, string unparsedValue,
            PropertyConfigurationAttribute propertyConfiguration) where T : class
        {
            object value;
            if (unparsedValue == null)
            {
                if (propertyConfiguration.IsMandatory)
                {
                    throw new MissingMandatoryConfigurationException(typeof(T), property.Name);
                }
                value = propertyConfiguration.DefaultValue;
            }
            else
            {
                value = Parse(property, unparsedValue, propertyConfiguration);
            }
            return value;
        }

        private object Parse(CachedProperty property, string unparsedValue,
            PropertyConfigurationAttribute propertyConfiguration)
        {
            var parser = GetParser(property.PropertyType, property.Name);
            object value;
            if (propertyConfiguration.AcceptInvalid)
            {
                value = parser.TryParse(unparsedValue,
                    propertyConfiguration.DefaultValue);
            }
            else
            {
                value = parser.Parse(unparsedValue);
            }
            return value;
        }

        private static bool IsIgnored(CachedProperty property)
        {
            var attribute =
                property.PropertyInfo.GetCustomAttribute<IgnoredPropertyAttribute>();
            return attribute != null;
        }

        private static PropertyConfigurationAttribute GetPropertyConfiguration(CachedProperty property)
        {
            return property.PropertyInfo.GetCustomAttribute<PropertyConfigurationAttribute>() ??
                   new PropertyConfigurationAttribute();
        }

        private string GetSectionName(CachedType type)
        {
            var sectionConfiguration = type.Info.GetCustomAttribute<SectionConfigurationAttribute>();
            if (sectionConfiguration == null)
            {
                return type.Name;
            }
            return sectionConfiguration.SectionName;
        }

        private IParser GetParser(Type type, string propertyName)
        {
            if (type == typeof(string))
            {
                return _noneParser;
            }
            if (type == typeof(int))
            {
                return new IntParser(type, propertyName);
            }
            if (type == typeof(bool))
            {
                return new BoolParser(type, propertyName);
            }
            throw new NotSupportedException($"{type} is not supported in IConfiguration");
        }
    }
}