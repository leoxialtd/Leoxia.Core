#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPAddressExtensionsTest.cs" company="Leoxia Ltd">
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

using System.Net;
using Xunit;

#endregion

namespace Leoxia.Network.Test
{
    public class IPAddressExtensionsTest
    {
        [Fact]
        public void IpAddressNetworkTest()
        {
            var address = IPAddress.Parse("10.0.0.1");
            Assert.Equal(IPNetworkType.Private, address.GetNetworkType());
            address = IPAddress.Parse("172.0.0.1");
            Assert.Equal(IPNetworkType.Public, address.GetNetworkType());
            address = IPAddress.Parse("10.0.2.245");
            Assert.Equal(IPNetworkType.Private, address.GetNetworkType());
            address = IPAddress.Parse("127.0.0.1");
            Assert.Equal(IPNetworkType.Loopback, address.GetNetworkType());
            address = IPAddress.Parse("0.0.0.0");
            Assert.Equal(IPNetworkType.Loopback, address.GetNetworkType());
        }

        [Fact]
        public void IpAddressLocationTest()
        {
            var address = IPAddress.Parse("10.0.0.1");
            Assert.False(address.GetLocationInformation().IsInRealPrivateNetwork);
            address = IPAddress.Parse("10.91.0.1");
            Assert.True(address.GetLocationInformation().IsInRealPrivateNetwork);
            address = IPAddress.Parse("10.91.0.1");
            Assert.False(address.GetLocationInformation().IsInDemilitarizedZone);
            address = IPAddress.Parse("163.172.114.67");
            Assert.True(address.GetLocationInformation().IsInDemilitarizedZone);
            address = IPAddress.Parse("163.172.114.67");
            Assert.True(address.GetLocationInformation().IsInDemilitarizedZone);
        }
    }
}