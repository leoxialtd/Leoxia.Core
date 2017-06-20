#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RSACertificateCipher.cs" company="Leoxia Ltd">
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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#endregion

namespace Leoxia.Security
{
    /// <summary>
    ///     Cipher based on RSA certificate keys
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class RSACertificateCipher
    {
        private readonly IX509CertificateProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RSACertificateCipher" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public RSACertificateCipher(IX509CertificateProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        ///     Encrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>encrypted string</returns>
        public string Encrypt(string input)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(input)));
        }

        /// <summary>
        ///     Encrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>encrypted bytes</returns>
        public byte[] Encrypt(byte[] input)
        {
            var certificate = _provider.Get();

            // GetRSAPublicKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (var rsa = certificate.GetRSAPublicKey())
            {
                return rsa.Encrypt(input, RSAEncryptionPadding.OaepSHA512);
            }
        }

        /// <summary>
        ///     Decrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>decrypted string</returns>
        public string Decrypt(string input)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(input)));
        }

        /// <summary>
        ///     Decrypts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>decrypted bytes</returns>
        /// <exception cref="System.InvalidOperationException">Certificate doesn't contain private key: cannot decrypt.</exception>
        public byte[] Decrypt(byte[] input)
        {
            var certificate = _provider.Get();

            // GetRSAPublicKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (var rsa = certificate.GetRSAPrivateKey())
            {
                if (rsa == null)
                {
                    throw new InvalidOperationException("Certificate doesn't contain private key: cannot decrypt.");
                }
                return rsa.Decrypt(input, RSAEncryptionPadding.OaepSHA512);
            }
        }
    }
}