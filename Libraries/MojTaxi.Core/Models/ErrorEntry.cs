using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Models
{

    public class ErrorEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string AppVersion { get; set; } = string.Empty;
    }
}
