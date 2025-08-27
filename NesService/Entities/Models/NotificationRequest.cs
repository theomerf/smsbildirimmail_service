using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class NotificationRequest : RequestBase
    {
        public String Title { get; set; }
        public String Body { get; set; }
        public Dictionary<String, String> Data { get; set; } = new Dictionary<string, string>();
        public virtual Request Request { get; set; }
    }
}
