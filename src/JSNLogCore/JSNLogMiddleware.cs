using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JSNLogCore
{
    public class JSNLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JSNLogOptions _options;

        public JSNLogMiddleware(RequestDelegate next, JSNLogOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context, ILogger<JSNLogMiddleware> logger)
        {
            if (context.Request.Path == _options.Route)
            {
                if (context.Request.Body.CanRead)
                {
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        var body = await reader.ReadToEndAsync();
                        JSNLogMessage jsnLogMessage;
                        try
                        {
                            jsnLogMessage = JsonConvert.DeserializeObject<JSNLogMessage>(body);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError($"Bad request with body: {body}. {ex.Message}");
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsync("Bad request");
                            return;
                        }

                        if (jsnLogMessage != null)
                        {
                            foreach (var jsnLogEntry in jsnLogMessage.Entries)
                            {
                                switch (jsnLogEntry.Level)
                                {
                                    case JSNLogLevel.Trace:
                                        logger.LogTrace(jsnLogEntry.Message);
                                        break;
                                    case JSNLogLevel.Debug:
                                        logger.LogDebug(jsnLogEntry.Message);
                                        break;
                                    case JSNLogLevel.Info:
                                        logger.LogInformation(jsnLogEntry.Message);
                                        break;
                                    case JSNLogLevel.Warn:
                                        logger.LogWarning(jsnLogEntry.Message);
                                        break;
                                    case JSNLogLevel.Error:
                                        logger.LogError(jsnLogEntry.Message);
                                        break;
                                    case JSNLogLevel.Fatal:
                                        logger.LogCritical(jsnLogEntry.Message);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                    }
                }
                await Task.FromResult("ok");
            }
            await _next(context);
        }
    }
}