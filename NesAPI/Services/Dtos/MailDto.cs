using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class MailDto
    {
        [StringLength(255, ErrorMessage = "'To' alanı maksimum 255 karakter olabilir.")]
        [Required(ErrorMessage = "'To' alanı boş olamaz.")]
        public String To { get; set; }
        [Required(ErrorMessage = "'From' alanı boş olamaz.")]
        [StringLength(255, ErrorMessage = "'From' alanı maksimum 255 karakter olabilir.")]
        public String From { get; set; }
        [Required(ErrorMessage = "'Subject' alanı boş olamaz.")]
        [StringLength(100, ErrorMessage = "'Subject' alanı maksimum 100 karakter olabilir.")]
        public String Subject { get; set; }
        [StringLength(255, ErrorMessage = "'Cc' alanı maksimum 255 karakter olabilir.")]
        public String Cc { get; set; }
        [StringLength(255, ErrorMessage = "'Bcc' alanı maksimum 255 karakter olabilir.")]
        public String Bcc { get; set; }
        [Required(ErrorMessage = "'Body' alanı boş olamaz.")]
        public String Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public ICollection<MailAttachmentDto> Attachments { get; set; } = new List<MailAttachmentDto>();
    }
}
