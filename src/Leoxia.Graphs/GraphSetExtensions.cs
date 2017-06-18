#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphSetExtensions.cs" company="Leoxia Ltd">
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
using System.Threading.Tasks;
using Leoxia.Collections;

#endregion

namespace Leoxia.Graphs
{
    /// <summary>
    ///     Extension methods on <see cref="GraphSet{T}" />
    /// </summary>
    public static class GraphSetExtensions
    {
        /// <summary>
        ///     Transform a <see cref="GraphSet{T}" /> to <see cref="GraphSet{TX}" />.
        /// </summary>
        /// <typeparam name="T">the type of element</typeparam>
        /// <typeparam name="TX">The type of element of the new <see cref="GraphSet{T}" />.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="func">The function.</param>
        /// <returns>new <see cref="GraphSet{T}" /></returns>
        public static GraphSet<TX> ToGraphSet<T, TX>(this IEnumerable<GraphNode<T>> enumerable, Func<T, TX> func)
        {
            var nodes = enumerable.ToArray();
            var dictionary = new Dictionary<GraphNode<T>, GraphNode<TX>>();
            var graphSet = new GraphSet<TX>();
            foreach (var item in nodes)
            {
                var node = graphSet.Add(func(item.Value));
                dictionary[item] = node;
            }
            foreach (var item in nodes)
            {
                var currentNode = dictionary[item];
                foreach (var child in item.Children)
                {
                    var childNode = dictionary[child];
                    currentNode.AddChild(childNode);
                }
                foreach (var parent in item.Parents)
                {
                    var parentNode = dictionary[parent];
                    currentNode.AddParent(parentNode);
                }
            }
            return graphSet;
        }

        /// <summary>
        ///     Determines whether the <see cref="GraphSet{T}" /> is cyclic.
        /// </summary>
        /// <typeparam name="T">type of element</typeparam>
        /// <param name="set">The set.</param>
        /// <returns>
        ///     <c>true</c> if the specified set is cyclic; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCyclic<T>(this GraphSet<T> set)
        {
            var heads = set.GetNodes().Where(x => x.Parents.Count == 0).ToList();
            if (heads.Count == 0)
            {
                return true;
            }
            var markedHeads = heads.Select(x => new MarkedNode<T>(x, new HashSet<GraphNode<T>>().ToIHashSet()));
            var stack = new Stack<MarkedNode<T>>(markedHeads);
            while (stack.Any())
            {
                var node = stack.Pop();
                if (node.MarkingSet.Contains(node.Node))
                {
                    return true;
                }
                node.MarkingSet.Add(node.Node);
                foreach (var child in node.Node.Children)
                {
                    stack.Push(new MarkedNode<T>(child, node.MarkingSet.Clone()));
                }
            }
            return false;
        }

        /// <summary>
        ///     Transforms a <see cref="GraphSet{T}" /> to a GraphSet of Task executing the <see cref="Action{T}" /> on
        ///     each of the nodes while respecting the graph order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set">The set.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        ///     <see cref="GraphSet{TaskPair}" />
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Cannot process task on a cyclic graph</exception>
        public static GraphSet<TaskPair<T>> ToTaskGraph<T>(this GraphSet<T> set, Action<T> action)
        {
            if (set.IsCyclic())
            {
                throw new InvalidOperationException("Cannot process task on a cyclic graph");
            }
            var taskSet = set.ToGraphSet(x => new TaskPair<T>(x));
            var nodes = taskSet.GetNodes().Where(n => n.Parents.Count == 0).ToArray();
            foreach (var node in nodes)
            {
                node.Value.Task = Task.Factory.StartNew(() => { action(node.Value.Value); });
            }
            var queue = new Queue<GraphNode<TaskPair<T>>>(nodes);
            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (node.Value.Task == null)
                {
                    var allParentsHaveTask = node.Parents.All(x => x.Value.Task != null);
                    if (allParentsHaveTask)
                    {
                        node.Value.Task = Task.Factory.ContinueWhenAll(
                            node.Parents.Select(x => x.Value.Task).ToArray(),
                            x => { action(node.Value.Value); });
                    }
                    queue.Enqueue(node);
                }
                else
                {
                    foreach (var child in node.Children)
                    {
                        if (child.Value.Task == null)
                        {
                            queue.Enqueue(child);
                        }
                    }
                }
            }
            return taskSet;
        }

        /// <summary>
        ///     ContinueWith specified <see cref="Action{T}" /> on each <see cref="Task" /> on the GraphSet
        /// </summary>
        /// <typeparam name="T">Type of the elements</typeparam>
        /// <param name="set">The set.</param>
        /// <param name="action">The action.</param>
        /// <returns><see cref="GraphSet{TaskPair}" /> with continuation tasks.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot process task on a cyclic graph</exception>
        public static GraphSet<TaskPair<T>> ContinueWith<T>(this GraphSet<TaskPair<T>> set, Action<T> action)
        {
            if (set.IsCyclic())
            {
                throw new InvalidOperationException("Cannot process task on a cyclic graph");
            }
            var tasks = new Dictionary<GraphNode<TaskPair<T>>, Task>();
            var taskSet = set.ToGraphSet(x => new TaskPair<T>(x.Value) {Task = x.Task});

            foreach (var node in taskSet.GetNodes())
            {
                tasks[node] = node.Value.Task;
                node.Value.Task = null;
            }
            var nodes = taskSet.GetNodes().Where(n => n.Parents.Count == 0).ToArray();
            foreach (var node in nodes)
            {
                node.Value.Task = tasks[node].ContinueWith(t => action(node.Value.Value));
            }
            var queue = new Queue<GraphNode<TaskPair<T>>>(nodes);
            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (node.Value.Task == null)
                {
                    var allParentsHaveTask = node.Parents.All(x => x.Value.Task != null);
                    if (allParentsHaveTask)
                    {
                        var list = node.Parents.Select(x => x.Value.Task).ToList();
                        list.Add(tasks[node]);
                        node.Value.Task = Task.Factory.ContinueWhenAll(
                            list.ToArray(),
                            x => action(node.Value.Value));
                    }
                    queue.Enqueue(node);
                }
                else
                {
                    foreach (var child in node.Children)
                    {
                        if (child.Value.Task == null)
                        {
                            queue.Enqueue(child);
                        }
                    }
                }
            }
            return taskSet;
        }

        /// <summary>
        ///     ContinueWith specified <see cref="Action{T}" /> on the leaves of the GraphSet.
        /// </summary>
        /// <typeparam name="T">Type of the elements</typeparam>
        /// <param name="set">The set.</param>
        /// <param name="action">The action.</param>
        /// <returns><see cref="GraphSet{TaskPair}" /> with continuation tasks.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot process task on a cyclic graph</exception>
        public static GraphSet<TaskPair<T>> ContinueOnLeavesWith<T>(this GraphSet<TaskPair<T>> set, Action<T> action)
        {
            if (set.IsCyclic())
            {
                throw new InvalidOperationException("Cannot process task on a cyclic graph");
            }
            var taskSet = set.ToGraphSet(x => new TaskPair<T>(x.Value) {Task = x.Task});
            var graphNodes = taskSet.GetNodes().Where(x => x.Children.Count == 0);
            foreach (var node in graphNodes)
            {
                node.Value.Task = node.Value.Task.ContinueWith(x => action(node.Value.Value));
            }
            return taskSet;
        }

        /// <summary>
        ///     Get the task corresponding to application of <see cref="Action{T}" /> on all nodes of
        ///     the <see cref="GraphSet{T}" />
        /// </summary>
        /// <typeparam name="T">type of the elements</typeparam>
        /// <param name="set">The set.</param>
        /// <param name="action">The action.</param>
        /// <returns>task</returns>
        public static Task ToTask<T>(this GraphSet<T> set, Action<T> action)
        {
            var taskSet = set.ToTaskGraph(action);
            return GetWhenAllTask(taskSet);
        }

        /// <summary>
        ///     Gets the when all task on a GraphSet of TaskPair
        /// </summary>
        /// <typeparam name="T">type of elements</typeparam>
        /// <param name="set">The set.</param>
        /// <returns>task</returns>
        public static Task GetWhenAllTask<T>(this GraphSet<TaskPair<T>> set)
        {
            var list = new List<Task>();
            foreach (var x in set.GetNodes())
            {
                list.Add(x.Value.Task);
            }
            var array = list.ToArray();
            return Task.Factory.ContinueWhenAll(array, tasks => { });
        }

        /// <summary>
        ///     Get a <see cref="GraphSet{T}" /> with only elements
        ///     satisfying the <see cref="Predicate{T}" />
        /// </summary>
        /// <typeparam name="T">the type of elements</typeparam>
        /// <param name="set">The set.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>new filtered <see cref="GraphSet{T}" /></returns>
        public static GraphSet<T> Filter<T>(this GraphSet<T> set, Predicate<T> predicate)
        {
            return new GraphSet<T>(set.GetNodes().Where(x => predicate(x.Value)), predicate);
        }
    }

    /// <summary>
    ///     Node keep track of node.
    /// </summary>
    /// <typeparam name="T">type of elements</typeparam>
    public class MarkedNode<T>
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public MarkedNode(GraphNode<T> node, IHashSet<GraphNode<T>> markingSet)
        {
            Node = node;
            MarkingSet = markingSet;
        }

        /// <summary>
        ///     Gets the marking set.
        /// </summary>
        /// <value>
        ///     The marking set.
        /// </value>
        public IHashSet<GraphNode<T>> MarkingSet { get; }

        /// <summary>
        ///     Gets the node.
        /// </summary>
        /// <value>
        ///     The node.
        /// </value>
        public GraphNode<T> Node { get; }
    }
}