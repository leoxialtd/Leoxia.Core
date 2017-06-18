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
using System.Linq;
using System.Reflection;
using Leoxia.Collections;

#endregion

namespace Leoxia.Reflection
{
    public class CachedProperty
    {
        private readonly Type _elementType;
        private IGetterMethod _getter;
        private ISetterMethod _setter;

        public CachedProperty(Type containingType, PropertyInfo propertyInfo)
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

        public Type ElementType => _elementType;
        public bool IsCollectionType { get; }

        public bool IsKey { get; } = false;

        public bool IsValidEntityType { get; }

        public bool IsVirtual { get; }
        public string Name => PropertyInfo.Name;
        public PropertyInfo PropertyInfo { get; }


        public Type PropertyType { get; }

        public TypeInfo PropertyTypeInfo { get; }

        public object GetValue(object newEntity)
        {
            return _getter.Invoke(newEntity);
        }

        public void SetValue(object instance, object value)
        {
            _setter.Invoke(instance, value);
        }

        public void Compile()
        {
            _getter = _getter.Compile();
            _setter = _setter.Compile();
        }
    }

    public class NotCompiledGetter : IGetterMethod
    {
        private readonly MethodInfo _method;

        public NotCompiledGetter(MethodInfo method)
        {
            _method = method;
        }

        public object Invoke(object instance)
        {
            return _method.Invoke(instance, EmptyArray<object>.Instance);
        }

        public IGetterMethod Compile()
        {
            return new CompiledGetter(_method);
        }
    }

    public class CompiledGetter : IGetterMethod
    {
        private readonly Delegate _delegate;

        public CompiledGetter(MethodInfo method)
        {
            var type = typeof(Func<,>).MakeGenericType(method.DeclaringType,
                method.ReturnType);
            _delegate = method.CreateDelegate(type);
        }

        public object Invoke(object instance)
        {
            return _delegate.DynamicInvoke(instance);
        }

        public IGetterMethod Compile()
        {
            return this;
        }
    }

    public interface IGetterMethod
    {
        object Invoke(object newEntity);
        IGetterMethod Compile();
    }

    public interface ISetterMethod
    {
        void Invoke(object instance, object value);
        ISetterMethod Compile();
    }

    public class NotCompiledSetter : ISetterMethod
    {
        private readonly MethodInfo _method;

        public NotCompiledSetter(MethodInfo method)
        {
            _method = method;
        }

        public void Invoke(object instance, object value)
        {
            _method.Invoke(instance, new[] {value});
        }

        public ISetterMethod Compile()
        {
            return new CompileSetter(_method);
        }
    }

    public class CompileSetter : ISetterMethod
    {
        private readonly Delegate _delegate;

        public CompileSetter(MethodInfo method)
        {
            var type = typeof(Action<,>).MakeGenericType(method.DeclaringType,
                method.GetParameters().FirstOrDefault().ParameterType);
            _delegate = method.CreateDelegate(type);
        }

        public void Invoke(object instance, object value)
        {
            _delegate.DynamicInvoke(instance, value);
        }

        public ISetterMethod Compile()
        {
            return this;
        }
    }
}