#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="X509CertificateSimpleProvider.cs" company="Leoxia Ltd">
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

using System.Security.Cryptography.X509Certificates;

namespace Leoxia.Security
{
    /// <summary>
    /// Provider of certificate that just take a <see cref="X509Certificate2"/> as input.
    /// </summary>
    /// <seealso cref="Leoxia.Security.IX509CertificateProvider" />
    public class X509CertificateSimpleProvider : IX509CertificateProvider
    {
        private readonly X509Certificate2 _certificate;

        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateSimpleProvider"/> class.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        public X509CertificateSimpleProvider(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        /// <summary>
        /// Gets the certificate.
        /// </summary>
        /// <returns>
        /// the certificate
        /// </returns>
        public X509Certificate2 Get()
        {
            return _certificate;
        }
    }
}