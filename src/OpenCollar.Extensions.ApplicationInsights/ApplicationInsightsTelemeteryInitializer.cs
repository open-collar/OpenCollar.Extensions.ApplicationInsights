/*
 * This file is part of OpenCollar.Extensions.Logging.
 *
 * OpenCollar.Extensions.Logging is free software: you can redistribute it
 * and/or modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * OpenCollar.Extensions.Logging is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public
 * License for more details.
 *
 * You should have received a copy of the GNU General Public License along with
 * OpenCollar.Extensions.Logging.  If not, see <https://www.gnu.org/licenses/>.
 *
 * Copyright © 2020 Jonathan Evans (jevans@open-collar.org.uk).
 */

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace OpenCollar.Extensions.Logging
{
    /// <summary>
    ///     A telemetry initializer that adds the contextual information captured by the <see cref="LoggingContext" />
    ///     into the custom data recorded by the ApplicationInsights logger. This makes it easier to search and filter
    ///     by these values in the online tools.
    /// </summary>
    /// <seealso cref="Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer" />
    public sealed class ApplicationInsightsTelemeteryInitializer : ITelemetryInitializer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApplicationInsightsTelemeteryInitializer" /> class.
        /// </summary>
        public ApplicationInsightsTelemeteryInitializer()
        {
            LoggerExtensions.AppendContextualInformationToLogMessages = false;
        }

        /// <summary>
        ///     Initializes properties of the specified <see cref="ApplicationInsightsTelemeteryInitializer" /> object.
        /// </summary>
        /// <param name="telemetry">
        ///     The telemetery object to which to append our custom values.
        /// </param>
        public void Initialize(ITelemetry telemetry)
        {
            LoggerExtensions.AppendContextualInformationToLogMessages = false;

            try
            {
                if(ReferenceEquals(telemetry, null))
                {
                    return;
                }

                var telemeteryContext = telemetry.Context;
                if(ReferenceEquals(telemeteryContext, null))
                {
                    return;
                }

                var properties = telemeteryContext.GlobalProperties;
                if(ReferenceEquals(properties, null))
                {
                    return;
                }

                var context = OpenCollar.Extensions.Logging.LoggingContext.Current();
                var snapshot = context.GetSnapshot();
                if(snapshot.Length <= 0)
                {
                    return;
                }

                foreach(var pair in snapshot)
                {
                    properties[@"Wtw.Crb.CarrierPortal." + pair.Key] = pair.Value;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // We absolutely, must not, impact the running of the application.
            }
        }
    }
}