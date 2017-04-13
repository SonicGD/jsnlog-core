using System.Collections.Generic;
using Newtonsoft.Json;

namespace JSNLogCore
{
    internal class JSNLogMessage
    {
        [JsonProperty("r")]
        public string RequestId { get; set; }

        [JsonProperty("lg")]
        public List<JSNLogEntry> Entries { get; set; }
    }

    internal class JSNLogEntry
    {
        [JsonProperty("l")]
        public JSNLogLevel Level { get; set; }

        [JsonProperty("m")]
        public string Message { get; set; }

        [JsonProperty("n")]
        public string LoggerName { get; set; }

        [JsonProperty("t")]
        public long Timestamp { get; set; }
    }

    internal enum JSNLogLevel
    {
        Trace = 1000,
        Debug = 2000,
        Info = 3000,
        Warn = 4000,
        Error = 5000,
        Fatal = 6000
    }
}