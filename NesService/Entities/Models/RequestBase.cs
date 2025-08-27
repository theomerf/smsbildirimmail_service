using System;

namespace Entities.Models
{
    public abstract class RequestBase
    {
        public Guid Id { get; set; }
        public String To { get; set; }
        public String From { get; set; }
        public DateTime CreatedAt { get; set; }
        public String CorrelationId { get; set; } = Guid.NewGuid().ToString();
    }
}
