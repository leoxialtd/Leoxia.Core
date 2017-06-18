#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphNode.cs" company="Leoxia Ltd">
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
using System.Linq;
using Leoxia.Collections;

#endregion

namespace Leoxia.Graphs
{
    /// <summary>
    ///     Node of a graph.
    /// </summary>
    /// <typeparam name="T">type of element</typeparam>
    /// <seealso>
    ///     <cref>System.IEquatable{Leoxia.Graphs.GraphNode{T}}</cref>
    /// </seealso>
    public struct GraphNode<T> : IEquatable<GraphNode<T>>
    {
        private readonly HashSet<GraphNode<T>> _parents;
        private readonly HashSet<GraphNode<T>> _children;
        private readonly GraphSet<T> _set;

        internal GraphNode(GraphSet<T> set, T value)
        {
            _set = set;
            _parents = new HashSet<GraphNode<T>>();
            _children = new HashSet<GraphNode<T>>();
            Value = value;
            set.Add(this);
        }


        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public T Value { get; }

        /// <summary>
        ///     Gets the parents.
        /// </summary>
        /// <value>
        ///     The parents.
        /// </value>
        public IHashSet<GraphNode<T>> Parents => new ReadOnlyHashSet<GraphNode<T>>(_parents);

        /// <summary>
        ///     Gets the children.
        /// </summary>
        /// <value>
        ///     The children.
        /// </value>
        public IHashSet<GraphNode<T>> Children => new ReadOnlyHashSet<GraphNode<T>>(_children);

        #region EqualityMembers

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(GraphNode<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise,
        ///     false.
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is GraphNode<T> && Equals((GraphNode<T>) obj);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        /// <summary>
        ///     Returns a value that indicates whether the values of two <see cref="T:Leoxia.Graphs.GraphNode`1" /> objects
        ///     are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise,
        ///     false.
        /// </returns>
        public static bool operator ==(GraphNode<T> left, GraphNode<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Returns a value that indicates whether two <see cref="T:Leoxia.Graphs.GraphNode`1" /> objects have different
        ///     values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(GraphNode<T> left, GraphNode<T> right)
        {
            return !left.Equals(right);
        }

        #endregion

        /// <summary>
        ///     Traverse the graph with depth first algorithm.
        /// </summary>
        /// <returns>the descendants with depth first order.</returns>
        public IEnumerable<GraphNode<T>> DepthFirst()
        {
            var list = new List<GraphNode<T>> {this};
            if (_children.Count == 0)
            {
                return list;
            }
            foreach (var child in _children)
            {
                list.AddRange(child.DepthFirst());
            }
            return list;
        }

        /// <summary>
        ///     Adds the parent in the parents collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Updated graph node</returns>
        public GraphNode<T> AddParent(T item)
        {
            var node = new GraphNode<T>(_set, item);
            return InnerAddParent(node);
        }

        private GraphNode<T> InnerAddParent(GraphNode<T> node)
        {
            _parents.Add(node);
            node._children.Add(this);
            return node;
        }

        /// <summary>
        ///     Adds the child in the children collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Updated graph node</returns>
        public GraphNode<T> AddChild(T item)
        {
            var node = new GraphNode<T>(_set, item);
            return InnerAddChild(node);
        }

        private GraphNode<T> InnerAddChild(GraphNode<T> node)
        {
            _children.Add(node);
            node._parents.Add(this);
            return node;
        }

        /// <summary>
        ///     Adds the parent to parents collection.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Updated graph node</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Node cannot be added as parent if it is from different
        ///     <see cref="GraphSet{T}" />
        /// </exception>
        public GraphNode<T> AddParent(GraphNode<T> node)
        {
            if (node._set != _set)
            {
                throw new InvalidOperationException("Node cannot be added as parent if it is from different GraphSet");
            }
            return InnerAddParent(node);
        }

        /// <summary>
        ///     Adds the child to the children collection.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Updated graph node</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Node cannot be added as child if it is from different
        ///     <see cref="GraphSet{T}" />
        /// </exception>
        public GraphNode<T> AddChild(GraphNode<T> node)
        {
            if (node._set != _set)
            {
                throw new InvalidOperationException("Node cannot be added as child if it is from different GraphSet");
            }
            return InnerAddChild(node);
        }

        /// <summary>
        ///     Traverse the graph with the leaves first algorithm.
        /// </summary>
        /// <returns>the ancestors with leaves first order.</returns>
        public IEnumerable<GraphNode<T>> LeavesFirst()
        {
            var list = new List<GraphNode<T>> {this};
            if (_parents.Count == 0)
            {
                return list;
            }
            foreach (var parent in _parents)
            {
                list.AddRange(parent.LeavesFirst());
            }
            return list;
        }

        /// <summary>
        ///     Determines whether the specified node has ancestor.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///     <c>true</c> if the specified node has ancestor; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAncestor(GraphNode<T> node)
        {
            if (_parents.Count == 0)
            {
                return false;
            }
            if (_parents.Contains(node))
            {
                return true;
            }
            return _parents.Any(x => x.HasAncestor(node));
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="T:System.String" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return $"[{_parents.Count}] {Value} [{_children.Count}]";
        }
    }
}