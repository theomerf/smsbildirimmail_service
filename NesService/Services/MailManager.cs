using MailKit.Net.Smtp;
using MimeKit;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services
{
    public class MailManager : IMailService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly String _smtpHost;
        private readonly String _smtpMail;
        private readonly String _smtpPassword;

        public MailManager(IRepositoryManager manager, ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;

            _smtpHost = ConfigurationManager.AppSettings["SmtpHost"];

            if (string.IsNullOrEmpty(_smtpHost))
            {
                throw new Exception("SMTP sağlayıcısı girilmedi");
            }

            _smtpMail = ConfigurationManager.AppSettings["SmtpMail"];

            if (string.IsNullOrEmpty(_smtpMail))
            {
                throw new Exception("SMTP maili girilmedi");
            }

            _smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

            if (string.IsNullOrEmpty(_smtpPassword))
            {
                throw new Exception("SMTP şifresi girilmedi");
            }
        }

        public async Task SendMailAsync(Guid id)
        {
            try
            {
                var mail = await _manager.Mail.GetMailById(id);

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Matlı.Net", mail.From));

                var receivers = mail.To.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var receiver in receivers)
                {
                    message.To.Add(new MailboxAddress("Kullanıcı", receiver.Trim()));
                }

                if (mail.Cc != null)
                {
                    var ccList = mail.Cc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cc in ccList)
                    {
                        message.Cc.Add(new MailboxAddress("Cc", cc));
                    }
                }
                if (mail.Bcc != null)
                {
                    var bccList = mail.Bcc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var bcc in bccList)
                    {
                        message.Bcc.Add(new MailboxAddress("Bcc", bcc));
                    }
                }

                message.Subject = mail.Subject;

                if (mail.Attachments != null && mail.Attachments.Any())
                {
                    var multipart = new Multipart("mixed");

                    TextPart textPart;
                    if (mail.IsBodyHtml)
                    {
                        textPart = new TextPart("html", mail.Body);
                    }
                    else
                    {
                        textPart = new TextPart("plain") { Text = mail.Body };
                    }
                    multipart.Add(textPart);


                    using (var httpClient = new HttpClient())
                    {
                        foreach (var attachment in mail.Attachments)
                        {
                            try
                            {
                                byte[] fileBytes;

                                if (Uri.IsWellFormedUriString(attachment.FilePath, UriKind.Absolute) &&
                                    (attachment.FilePath.StartsWith("http://") || attachment.FilePath.StartsWith("https://")))
                                {
                                    fileBytes = await httpClient.GetByteArrayAsync(attachment.FilePath);
                                }
                                else
                                {
                                    if (!File.Exists(attachment.FilePath))
                                    {
                                        _logger.LogWarning($"{id} için ek dosya bulunamadı: {attachment.FilePath}");
                                        continue;
                                    }
                                    fileBytes = File.ReadAllBytes(attachment.FilePath);
                                }

                                var attachmentMail = new MimePart("application", "octet-stream")
                                {
                                    Content = new MimeContent(new MemoryStream(fileBytes)),
                                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment)
                                    {
                                        Size = fileBytes.Length
                                    },
                                    ContentTransferEncoding = ContentEncoding.Base64,
                                    FileName = attachment.FileName
                                };

                                multipart.Add(attachmentMail);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"{id}'sine sahip maile ekler eklenirken hata oluştu: {ex.Message}");
                            }

                        }

                    }
                    message.Body = multipart;
                }
                else
                {
                    message.Body = mail.IsBodyHtml
                        ? new TextPart("html", mail.Body)
                        : new TextPart("plain", mail.Body);
                }

                using (var client = new SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync(_smtpHost, 587, MailKit.Security.SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync(_smtpMail, _smtpPassword);
                        await client.SendAsync(message);
                    }
                    catch
                    {
                        throw;
                    }

                    try
                    {
                        await client.DisconnectAsync(true);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Mail bağlantı kapatma hatası: {ex.Message}");
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
