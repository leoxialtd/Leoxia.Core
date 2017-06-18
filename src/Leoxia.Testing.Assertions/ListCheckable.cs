#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListCheckable.cs" company="Leoxia Ltd">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leoxia.Testing.Assertions.Abstractions;
using Leoxia.Testing.Assertions.Failures;

#endregion

namespace Leoxia.Testing.Assertions
{
    public class CheckFailure<T> : ICheckFailure<T>
    {
    }

    public interface ICheckFailure<T>
    {
    }

    public class ListCheckable : BaseClassCheckable<IList>, IListCheckable
    {
        public ListCheckable(IExceptionFactory factory, IList value)
            : base(factory, value)
        {
        }

        protected override bool InnerIsEqualTo(IList expected, string message = null)
        {
            if (_value.Count != expected.Count)
            {
                var checkFailure = new ListCheckFailure<IList>(CheckType.CountEqual, _value, expected, message);
                checkFailure.ExpectedCount = expected.Count;
                checkFailure.TestedCount = _value.Count;
                throw _factory.Build(checkFailure);
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (_value[i] != expected[i])
                {
                    var checkFailure = new ListCheckFailure<IList>(CheckType.ListItemEqual, _value, expected, message);
                    checkFailure.Index = i;
                    throw _factory.Build(checkFailure);
                }
            }
            return true;
        }

        protected override bool InnerIsNotEqualTo(IList expected, string message = null)
        {
            if (_value.Count != expected.Count)
            {
                return true;
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (_value[i] != expected[i])
                {
                    return true;
                }
            }
            var checkFailure = new ListCheckFailure<IList>(CheckType.ListItemNotEqual, _value, expected, message);
            throw _factory.Build(checkFailure);
        }
    }

    public class ListCheckFailure<T> : BaseCheckFailure<T>
    {
        public ListCheckFailure(CheckType type, T tested, T expected, string message) : base(type, tested, expected,
            message)
        {
        }

        public int ExpectedCount { get; set; }
        public int Index { get; set; }
        public int TestedCount { get; set; }

        protected override string DisplayMessage()
        {
            switch (_type)
            {
                case CheckType.CountEqual:
                {
                    return $"Check that {_tested} is equal to {_expected} but item count is different:" +
                           Environment.NewLine +
                           $"Tested.Count == {TestedCount} and Expected.Count == {ExpectedCount}";
                }
                case CheckType.ListItemEqual:
                {
                    return $"Check that {_tested} is equal to {_expected} but items are different on index {Index}:" +
                           Environment.NewLine +
                           $"Tested[{Index}] == {((IList) _tested)[Index]} and Expected[{Index}] == {((IList) _expected)[Index]}";
                }
                case CheckType.ListItemNotEqual:
                {
                    return $"Check that {_tested} is not equal to {_expected} but all items are equal";
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public class ListCheckable<T> : BaseClassCheckable<IList<T>>, IListCheckable<T>
    {
        public ListCheckable(IExceptionFactory factory, IList<T> value) : base(factory, value)
        {
        }

        public void Contains(T item, string message = null)
        {
            if (!_value.Contains(item))
            {
                var checkFailure = new ListItemCheckFailure<T>(CheckType.ListItemIsNotContained, _value, item, message);
                throw _factory.Build(checkFailure);
            }
        }

        protected override bool InnerIsEqualTo(IList<T> expected, string message = null)
        {
            if (_value.Count != expected.Count)
            {
                var checkFailure = new ListCheckFailure<IList<T>>(CheckType.CountEqual, _value, expected, message);
                checkFailure.ExpectedCount = expected.Count;
                checkFailure.TestedCount = _value.Count;
                throw _factory.Build(checkFailure);
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (!_value[i].Equals(expected[i]))
                {
                    var checkFailure =
                        new ListCheckFailure<IList<T>>(CheckType.ListItemEqual, _value, expected, message);
                    checkFailure.Index = i;
                    throw _factory.Build(checkFailure);
                }
            }
            return true;
        }

        protected override bool InnerIsNotEqualTo(IList<T> expected, string message = null)
        {
            if (_value.Count == expected.Count)
            {
                return true;
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (!_value[i].Equals(expected[i]))
                {
                    return true;
                }
            }
            var checkFailure = new ListCheckFailure<IList<T>>(CheckType.ListItemAllEqual, _value, expected, message);
            throw _factory.Build(checkFailure);
        }
    }

    public class ListItemCheckFailure<T> : BaseCheckFailure<T>
    {
        private new readonly IList<T> _tested;

        public ListItemCheckFailure(CheckType checkType, IList<T> tested, T expected, string message)
            : base(checkType, default(T), expected, message)
        {
            _tested = tested;
        }

        protected override string DisplayMessage()
        {
            return DisplayItem(_expected) + " is not contained in [" + DisplayList() + "]";
        }

        private string DisplayList()
        {
            return string.Join(", ", _tested.Select(DisplayItem));
        }

        private static string DisplayItem(T x)
        {
            if (x == null)
            {
                return "Null";
            }
            var res = x.ToString();
            if (res == null)
            {
                return "Null";
            }
            if (res.Length == 0)
            {
                return "Empty";
            }
            return res;
        }
    }
}