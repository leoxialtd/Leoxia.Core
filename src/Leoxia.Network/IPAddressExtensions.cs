#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPAddressExtensions.cs" company="Leoxia Ltd">
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

using System.Net;
using System.Net.Sockets;
using DnsClient;

namespace Leoxia.Network
{
    public static class IpAddressExtensions
    {
        private static readonly LookupClient _client =
            new LookupClient(IPAddress.Parse("8.8.8.8"), IPAddress.Parse("8.8.4.4"));


        public static IPNetworkType GetNetworkType(this IPAddress address)
        {
            switch (address.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                {
                    byte[] bytes = address.GetAddressBytes();
                    switch (bytes[0])
                    {
                        case 0:
                        {
                            return IPNetworkType.Loopback;
                        }
                        case 10:
                        {
                            return IPNetworkType.Private;
                        }
                        case 100:
                        {
                            if (bytes[1] > 63 && bytes[1] < 128)
                            {
                                return IPNetworkType.PrivateIsp;
                            }
                            break;
                        }
                        case 127:
                        {
                            return IPNetworkType.Loopback;
                        }
                        case 172:
                        {
                            if (bytes[1] > 15 && bytes[1] < 32)
                            {
                                return IPNetworkType.Private;
                            }
                            break;
                        }
                        case 192:
                        {
                            if (bytes[1] == 0)
                            {
                                if (bytes[2] == 0)
                                {
                                    return IPNetworkType.IANARegistry;
                                }
                                if (bytes[2] == 2)
                                {
                                    return IPNetworkType.Documentation;
                                }
                            }
                            if (bytes[1] == 168)
                            {
                                return IPNetworkType.Private;
                            }
                            break;
                        }
                        case 198:
                        {
                            if (bytes[1] == 18)
                            {
                                return IPNetworkType.PrivateTest;
                            }
                            if (bytes[1] == 51 && bytes[2] == 100)
                            {
                                return IPNetworkType.Documentation;
                            }
                            break;
                        }
                        case 203:
                        {
                            if (bytes[1] == 0 && bytes[2] == 113)
                            {
                                return IPNetworkType.Documentation;
                            }
                            break;
                        }
                        case 240:
                        {
                            return IPNetworkType.Reserved;
                        }
                    }
                    // Class D
                    if (bytes[0] > 223 && bytes[0] < 240)
                    {
                        return IPNetworkType.Multicast;
                    }
                    // Class E
                    if (bytes[0] > 240)
                    {
                        if (bytes[1] == 255 && bytes[2] == 255 && bytes[3] == 255)
                        {
                            return IPNetworkType.LimitedBroadcast;
                        }
                        return IPNetworkType.Reserved;
                    }
                    break;
                }
                case AddressFamily.InterNetworkV6:
                    break;
                default:
                    break;
            }
            return IPNetworkType.Public;
        }

        public static IPLocationInformation GetLocationInformation(this IPAddress address)
        {
            var information = new IPLocationInformation();
            information.NetworkType = address.GetNetworkType();
            if (information.NetworkType == IPNetworkType.Private)
            {
                var addressBytes = address.GetAddressBytes();
                if (addressBytes[0] == 10 && addressBytes[1] == 91)
                {
                    information.IsInRealPrivateNetwork = true;
                }
            }
            if (information.NetworkType == IPNetworkType.Public)
            {
                information.IsInDemilitarizedZone = true;
            }
            return information;
        }
    }
}