    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Entities.Models
    {
        public class SmsRequest : RequestBase
        {
            public String Message { get; set; }
            public virtual Request Request { get; set; }

            public override bool Validate(out string error)
            {
                if (!base.Validate(out error)) return false;

                if (string.IsNullOrWhiteSpace(Message))
                {
                    error = "'Message' boş olamaz.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(From))
                {
                    error = "'From' alanı boş olamaz.";
                    return false;
                }

            return true;
            }
        }
    }
