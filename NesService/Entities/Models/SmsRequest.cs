    using System;

    namespace Entities.Models
    {
        public class SmsRequest : RequestBase
        {
            public String Message { get; set; }
            public virtual Request Request { get; set; }
        }
    }
