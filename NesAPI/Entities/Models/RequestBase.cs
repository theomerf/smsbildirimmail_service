using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public abstract class RequestBase
    {
        public Guid Id { get; set; }
        public String To { get; set; }
        public String From { get; set; }
        public DateTime CreatedAt { get; set; }
        public String CorrelationId { get; set; } = Guid.NewGuid().ToString();

        public virtual bool Validate(out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(To))
            {
                error = "'To' alanı boş olamaz.";
                return false;
            }
            return true;
        }
    }
}
