using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class NotificationRequest : RequestBase
    {
        public String Title { get; set; }
        public String Body { get; set; }
        public Dictionary<String, String> Data { get; set; } = new Dictionary<string, string>();
        public virtual Request Request { get; set; }

        public override bool Validate(out string error)
        {
            if (!base.Validate(out error)) return false;

            if (string.IsNullOrWhiteSpace(Title))
            {
                error = "'Title' boş olamaz.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Body))
            {
                error = "'Body' boş olamaz";
                return false;
            }
            return true;
        }
    }
}
