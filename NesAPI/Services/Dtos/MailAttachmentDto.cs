using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class MailAttachmentDto
    {
        [Required(ErrorMessage = "'FileName' alanı boş olamaz.")]
        [StringLength(255, ErrorMessage = "'FileName' alanı maksimum 255 karakter olabilir.")]
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }

        [Required(ErrorMessage = "'FilePath' alanı boş olamaz.")]
        public string FilePath { get; set; }
    }
}
