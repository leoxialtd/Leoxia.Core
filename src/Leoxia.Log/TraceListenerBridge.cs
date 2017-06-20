#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TraceListenerBridge.cs" company="Leoxia Ltd">
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
using System.Diagnostics;
using System.Text;

#endregion

namespace Leoxia.Log
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.Diagnostics.TraceListener" />
    public class TraceListenerBridge : TraceListener
    {
        private static readonly ILogger _logger = LogManager.GetLogger("Global");


        /// <summary>
        ///     When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            _logger.Info(message);
        }


        /// <summary>
        ///     Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="message"></param>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override void WriteLine(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        ///     Traces the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="data">The data.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
            object data)
        {
            Log(eventCache, source, eventType, id, ToMessage(data));
        }

        private void Log(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            var logEvent = new LogEvent(id, ConvertEventType(eventType), source, message, eventCache.DateTime,
                eventCache.Timestamp, eventCache.ThreadId, eventCache.ProcessId);
            LogManager.AppenderMediator.Log(logEvent);
        }

        private string ToMessage(object data)
        {
            if (data == null)
            {
                return "NULL";
            }
            return data.ToString();
        }

        private static LogLevel ConvertEventType(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Critical:
                    return LogLevel.Fatal;
                case TraceEventType.Error:
                    return LogLevel.Error;
                case TraceEventType.Information:
                    return LogLevel.Info;
                case TraceEventType.Verbose:
                    return LogLevel.Debug;
                case TraceEventType.Warning:
                    return LogLevel.Warn;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }

        /// <summary>
        ///     Writes trace information, an array of data objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">
        ///     A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process
        ///     ID, thread ID, and stack trace information.
        /// </param>
        /// <param name="source">
        ///     A name used to identify the output, typically the name of the application that generated the trace
        ///     event.
        /// </param>
        /// <param name="eventType">
        ///     One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of
        ///     event that has caused the trace.
        /// </param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">An array of objects to emit as data.</param>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
            params object[] data)
        {
            Log(eventCache, source, eventType, id, ToMessages(data));
        }

        private string ToMessages(object[] data)
        {
            var builder = new StringBuilder();
            foreach (var item in data)
            {
                builder.Append(ToMessage(item) + "/");
            }
            return builder.ToString();
        }

        /// <summary>
        ///     Writes trace and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">
        ///     A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process
        ///     ID, thread ID, and stack trace information.
        /// </param>
        /// <param name="source">
        ///     A name used to identify the output, typically the name of the application that generated the trace
        ///     event.
        /// </param>
        /// <param name="eventType">
        ///     One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of
        ///     event that has caused the trace.
        /// </param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            Log(eventCache, source, eventType, id, string.Empty);
        }


        /// <summary>
        ///     Traces the event.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="message">The message.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
            string message)
        {
            Log(eventCache, source, eventType, id, message);
        }


        /// <summary>
        ///     Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">
        ///     A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process
        ///     ID, thread ID, and stack trace information.
        /// </param>
        /// <param name="source">
        ///     A name used to identify the output, typically the name of the application that generated the trace
        ///     event.
        /// </param>
        /// <param name="eventType">
        ///     One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of
        ///     event that has caused the trace.
        /// </param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
            string format, params object[] args)
        {
            Log(eventCache, source, eventType, id, string.Format(format, args));
        }
    }
}