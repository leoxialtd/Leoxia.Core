#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationTest.cs" company="Leoxia Ltd">
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

using Microsoft.Extensions.Configuration;
using Xunit;

#endregion

namespace Leoxia.Configuration.Test
{
    public class ConfigurationTest
    {
        [Fact]
        public void ReadConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            var holder = reader.Read<ConfigurationHolder>();
            Assert.Equal("mystring", holder.MyString);
            Assert.Equal(true, holder.MyFlag);
            Assert.Equal(42, holder.MyNumber);
        }

        [Fact]
        public void ReadConfigurationOnInstance()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            var holder = new ConfigurationHolder();
            reader.Read(holder);
            Assert.Equal("mystring", holder.MyString);
            Assert.Equal(true, holder.MyFlag);
            Assert.Equal(42, holder.MyNumber);
        }

        [Fact]
        public void ReadMissingConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            Assert.Throws(typeof(MissingMandatoryConfigurationException),
                () => { reader.Read<MissingConfigurationHolder>(); });
        }

        [Fact]
        public void ReadMissingDefaultingConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            var holder = reader.Read<MissingDefaultingConfigurationHolder>();
            Assert.Equal("MyDefault", holder.MyMissingProperty);
        }

        [Fact]
        public void ReadMissingIgnoredConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            reader.Read<MissingIgnoredConfigurationHolder>();
        }

        [Fact]
        public void ReadInvalidIgnoredConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            reader.Read<InvalidIgnoredConfigurationHolder>();
        }

        [Fact]
        public void ReadInvalidBoolConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            Assert.Throws(typeof(InvalidConfigurationException),
                () => { reader.Read<InvalidBoolConfigurationHolder>(); });
        }

        [Fact]
        public void ReadInvalidBoolDefaultingConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            var holder = reader.Read<InvalidBoolDefaultingConfigurationHolder>();
            Assert.True(holder.MyString);
        }

        [Fact]
        public void ReadInvalidIntConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("mailservices.json");
            var reader = new ConfigurationReader(builder.Build());
            Assert.Throws(typeof(InvalidConfigurationException),
                () => { reader.Read<InvalidIntConfigurationHolder>(); });
        }
    }
}