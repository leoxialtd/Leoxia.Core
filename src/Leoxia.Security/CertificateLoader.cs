#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CertificateLoader.cs" company="Leoxia Ltd">
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

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Leoxia.Abstractions.IO;
using Leoxia.IO;
using Leoxia.Log;

#endregion

namespace Leoxia.Security
{
    /// <summary>
    ///     Loads a X509 Certificate from a file.
    /// </summary>
    public class CertificateLoader
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(CertificateLoader));
        private readonly IPathMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CertificateLoader" /> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public CertificateLoader(IPathMapper mapper = null)
        {
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets the certificate.
        /// </summary>
        /// <value>
        ///     The certificate.
        /// </value>
        public X509Certificate2 Load(IFileInfo certificatePath, string passPhrase)
        {
            var mappedPath = _mapper != null ? _mapper.Map(certificatePath) : certificatePath;
            try
            {
                var reader = new FileReader();
                byte[] bytes = reader.ReadBytes(mappedPath);
                // The key storage is really important as without it, it won't work when run from IIS
                var certificate = new X509Certificate2(bytes, passPhrase,
                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
                _logger.InfoFormat("Certificate loaded from {0}", mappedPath);
                return certificate;
            }
            catch (CryptographicException e)
            {
                // Get the path in the exception
                throw new CryptographicException("In " + mappedPath + ": " + e.Message);
            }
        }
    }
}