#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpResolver.cs" company="Leoxia Ltd">
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

using System.Linq;
using System.Net;
using DnsClient;

namespace Leoxia.Network
{
    /// <summary>
    /// Resolve Ip from DNS.
    /// </summary>
    public class IpResolver
    {
        private readonly LookupClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="IpResolver"/> class.
        /// </summary>
        /// <param name="domainNameServerIps">The domain name server ips.</param>
        public IpResolver(params string[] domainNameServerIps)
        {
            var servers = domainNameServerIps.Select(IPAddress.Parse).ToArray();
            _client = new LookupClient(servers);
        }

        /// <summary>
        /// Resolves the domain name to an ip.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <returns></returns>
        public string ResolveIp(string domainName)
        {
            var response = _client.Query(domainName, QueryType.A);
            var answer = response.Answers.FirstOrDefault();
            return answer?.RecordToString();
        }
    }
}