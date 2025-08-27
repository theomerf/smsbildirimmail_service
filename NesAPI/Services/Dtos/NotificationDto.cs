using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class NotificationDto
    {
        [StringLength(255, ErrorMessage = "'To' alanı maksimum 255 karakter olabilir.")]
        [Required(ErrorMessage = "'To' alanı boş olamaz.")]
        public String To { get; set; }
        [StringLength(255, ErrorMessage = "'Title' alanı maksimum 100 karakter olabilir.")]
        [Required(ErrorMessage = "'Title' alanı boş olamaz.")]
        public String Title { get; set; }
        [Required(ErrorMessage = "'Body' alanı boş olamaz.")]
        public String Body { get; set; }
        public Dictionary<String, String> Data { get; set; } = new Dictionary<string, string>();
    }
}
