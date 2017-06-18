#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpNetworkType.cs" company="Leoxia Ltd">
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

namespace Leoxia.Network
{
    /// <summary>
    ///     Type of network, an ip can belong to
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public enum IPNetworkType
    {
        /// <summary>
        ///     The default if not from other types
        /// </summary>
        Public,

        /// <summary>
        ///     Private networks
        /// </summary>
        Private,

        /// <summary>
        ///     Used for communications between a service provider and its subscribers when using a carrier-grade NAT
        /// </summary>
        PrivateIsp,

        /// <summary>
        ///     The host
        /// </summary>
        Loopback,

        /// <summary>
        ///     Used for link-local addresses between two hosts on a single link when no IP address is otherwise specified, such as
        ///     would have normally been retrieved from a DHCP server
        /// </summary>
        Subnet,

        /// <summary>
        ///     Not to be used publicly. Assigned as "TEST-NET" for use in documentation and examples.
        /// </summary>
        Documentation,

        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     Used for the IANA IPv4 Special Purpose Address Registry
        /// </summary>
        IANARegistry,

        /// <summary>
        ///     Used for testing of inter-network communications between two separate subnets
        /// </summary>
        PrivateTest,

        /// <summary>
        ///     Reserved for multicast
        /// </summary>
        Multicast,

        /// <summary>
        ///     Reserved for future use
        /// </summary>
        Reserved,

        /// <summary>
        ///     Reserved for the "limited broadcast" destination address
        /// </summary>
        LimitedBroadcast
    }
}