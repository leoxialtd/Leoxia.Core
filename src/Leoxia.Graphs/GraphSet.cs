#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphSet.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Graphs
{
    /// <summary>
    ///     Set of graph nodes.
    /// </summary>
    /// <typeparam name="T">type of element</typeparam>
    /// <seealso>
    ///     <cref>System.Collections.Generic.IEnumerable{Leoxia.Graphs.GraphNode{T}}</cref>
    /// </seealso>
    public class GraphSet<T> : IEnumerable<GraphNode<T>>
    {
        private readonly IList<GraphNode<T>> _nodes = new List<GraphNode<T>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="GraphSet{T}" /> class.
        /// </summary>
        public GraphSet()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GraphSet{T}" /> class.
        /// </summary>
        /// <param name="set">The set.</param>
        public GraphSet(GraphSet<T> set) : this(set._nodes, x => true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GraphSet{T}" /> class.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="predicate">The predicate.</param>
        public GraphSet(IEnumerable<GraphNode<T>> nodes, Predicate<T> predicate)
        {
            var references = new Dictionary<GraphNode<T>, GraphNode<T>>();
            foreach (var node in nodes)
            {
                GraphNode<T> newNode;
                if (!references.TryGetValue(node, out newNode))
                {
                    newNode = Add(node.Value);
                    references[node] = newNode;
                }
                foreach (var child in node.Children)
                {
                    if (predicate(child.Value))
                    {
                        GraphNode<T> newChild;
                        if (!references.TryGetValue(child, out newChild))
                        {
                            newChild = Add(child.Value);
                            references[child] = newChild;
                        }
                        newNode.AddChild(newChild);
                    }
                }
                foreach (var parent in node.Parents)
                {
                    if (predicate(parent.Value))
                    {
                        GraphNode<T> newParent;
                        if (!references.TryGetValue(parent, out newParent))
                        {
                            newParent = Add(parent.Value);
                            references[parent] = newParent;
                        }
                        newNode.AddParent(newParent);
                    }
                }
            }
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<GraphNode<T>> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>the graph node added</returns>
        // ReSharper disable once MethodNameNotMeaningful
        public GraphNode<T> Add(T value)
        {
            var node = new GraphNode<T>(this, value);
            return node;
        }

        // ReSharper disable once MethodNameNotMeaningful
        internal void Add(GraphNode<T> node)
        {
            _nodes.Add(node);
        }

        /// <summary>
        ///     Gets the nodes.
        /// </summary>
        /// <returns>the nodes</returns>
        public IEnumerable<GraphNode<T>> GetNodes()
        {
            return _nodes;
        }
    }
}