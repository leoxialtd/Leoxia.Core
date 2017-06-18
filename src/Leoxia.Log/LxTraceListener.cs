#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LxTraceListener.cs" company="Leoxia Ltd">
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
    public class LxTraceListener : TraceListener
    {
        private static readonly ILogger _logger = LogManager.GetLogger("Global");

        /// <summary>
        ///     En cas de substitution dans une classe dérivée, écrit le message spécifié dans l'écouteur que vous créez dans la
        ///     classe dérivée.
        /// </summary>
        /// <param name="message">Message à écrire. </param>
        public override void Write(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        ///     En cas de substitution dans une classe dérivée, écrit un message dans l'écouteur que vous créez dans la classe
        ///     dérivée, suivi d'une marque de fin de ligne.
        /// </summary>
        /// <param name="message">Message à écrire. </param>
        public override void WriteLine(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        ///     Écrit les informations sur la trace, un objet de données et les informations sur les événements dans la sortie
        ///     spécifique de l'écouteur.
        /// </summary>
        /// <param name="eventCache">
        ///     Objet <see cref="T:System.Diagnostics.TraceEventCache" /> qui contient les informations
        ///     actuelles sur l'ID de processus, l'ID de thread et la trace de la pile.
        /// </param>
        /// <param name="source">
        ///     Nom utilisé pour identifier la sortie, généralement le nom de l'application qui a généré
        ///     l'événement de trace.
        /// </param>
        /// <param name="eventType">
        ///     Une des valeurs de <see cref="T:System.Diagnostics.TraceEventType" /> spécifiant le type
        ///     d'événement qui a déclenché la trace.
        /// </param>
        /// <param name="id">Identificateur numérique pour l'événement.</param>
        /// <param name="data">Données de trace à émettre.</param>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
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
        ///     Écrit les informations sur la trace, un tableau d'objets de données et les informations sur les événements dans la
        ///     sortie spécifique de l'écouteur.
        /// </summary>
        /// <param name="eventCache">
        ///     Objet <see cref="T:System.Diagnostics.TraceEventCache" /> qui contient les informations
        ///     actuelles sur l'ID de processus, l'ID de thread et la trace de la pile.
        /// </param>
        /// <param name="source">
        ///     Nom utilisé pour identifier la sortie, généralement le nom de l'application qui a généré
        ///     l'événement de trace.
        /// </param>
        /// <param name="eventType">
        ///     Une des valeurs de <see cref="T:System.Diagnostics.TraceEventType" /> spécifiant le type
        ///     d'événement qui a déclenché la trace.
        /// </param>
        /// <param name="id">Identificateur numérique pour l'événement.</param>
        /// <param name="data">Tableau d'objets à émettre comme données.</param>
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
        ///     Écrit les informations sur la trace et les événements dans la sortie spécifique de l'écouteur.
        /// </summary>
        /// <param name="eventCache">
        ///     Objet <see cref="T:System.Diagnostics.TraceEventCache" /> qui contient les informations
        ///     actuelles sur l'ID de processus, l'ID de thread et la trace de la pile.
        /// </param>
        /// <param name="source">
        ///     Nom utilisé pour identifier la sortie, généralement le nom de l'application qui a généré
        ///     l'événement de trace.
        /// </param>
        /// <param name="eventType">
        ///     Une des valeurs de <see cref="T:System.Diagnostics.TraceEventType" /> spécifiant le type
        ///     d'événement qui a déclenché la trace.
        /// </param>
        /// <param name="id">Identificateur numérique pour l'événement.</param>
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
        ///     Écrit les informations sur la trace, un message et les informations sur les événements dans la sortie spécifique de
        ///     l'écouteur.
        /// </summary>
        /// <param name="eventCache">
        ///     Objet <see cref="T:System.Diagnostics.TraceEventCache" /> qui contient les informations
        ///     actuelles sur l'ID de processus, l'ID de thread et la trace de la pile.
        /// </param>
        /// <param name="source">
        ///     Nom utilisé pour identifier la sortie, généralement le nom de l'application qui a généré
        ///     l'événement de trace.
        /// </param>
        /// <param name="eventType">
        ///     Une des valeurs de <see cref="T:System.Diagnostics.TraceEventType" /> spécifiant le type
        ///     d'événement qui a déclenché la trace.
        /// </param>
        /// <param name="id">Identificateur numérique pour l'événement.</param>
        /// <param name="message">Message à écrire.</param>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
            string message)
        {
            Log(eventCache, source, eventType, id, message);
        }

        /// <summary>
        ///     Écrit les informations sur la trace, un tableau d'objets mis en forme et les informations sur les événements dans
        ///     la sortie spécifique de l'écouteur.
        /// </summary>
        /// <param name="eventCache">
        ///     Objet <see cref="T:System.Diagnostics.TraceEventCache" /> qui contient les informations
        ///     actuelles sur l'ID de processus, l'ID de thread et la trace de la pile.
        /// </param>
        /// <param name="source">
        ///     Nom utilisé pour identifier la sortie, généralement le nom de l'application qui a généré
        ///     l'événement de trace.
        /// </param>
        /// <param name="eventType">
        ///     Une des valeurs de <see cref="T:System.Diagnostics.TraceEventType" /> spécifiant le type
        ///     d'événement qui a déclenché la trace.
        /// </param>
        /// <param name="id">Identificateur numérique pour l'événement.</param>
        /// <param name="format">
        ///     Chaîne de format qui contient zéro élément de format ou plus, lesquels correspondent aux objets
        ///     dans le tableau <paramref name="args" />.
        /// </param>
        /// <param name="args">Tableau object qui contient zéro objet ou plus à mettre en forme.</param>
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