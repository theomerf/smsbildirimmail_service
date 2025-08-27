using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class SmsDto
    {
        [StringLength(255, ErrorMessage = "'To' alanı maksimum 255 karakter olabilir.")]
        [Required(ErrorMessage = "'To' alanı boş olamaz.")]
        public String To { get; set; }
        [Required(ErrorMessage = "'From' alanı boş olamaz.")]
        [StringLength(255, ErrorMessage = "'From' alanı maksimum 255 karakter olabilir.")]
        public String From { get; set; }
        [Required(ErrorMessage = "'Message' alanı boş olamaz.")]
        public String Message { get; set; }
    }
}
