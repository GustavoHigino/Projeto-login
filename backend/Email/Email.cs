using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace Api_para_login.Email
{
    public class Email
    {
        public string Provedor { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public Email(string provedor, string username,string password)
        {
            Provedor = provedor;
            Username = username;
            Password = password;
        }
        public void SendEmail(List<string> emailsto,string subject, string body, List<string> attachments)
        {
            var message = PrepareteMessage(emailsto, subject, body, attachments);
            SendEmailBySmtp(message);
        }
        private MailMessage PrepareteMessage(List<string> emailsto, string subject, string body, List<string> attachments)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(Username);
            foreach(var item in emailsto)
            {
                if (ValidateEmail(item))
                {
                    mail.To.Add(item);
                }
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            foreach (var file in attachments)
            {
                var data = new Attachment(file, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                mail.Attachments.Add(data);
            }
            return mail;
        }
        private bool ValidateEmail(string email)
        {
            Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
            if (expression.IsMatch(email))
            {
                return true;
            }
            return false;
        }
        private void SendEmailBySmtp(MailMessage message)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = Provedor;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 50000;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(Username, Password);
            smtpClient.Send(message);
            smtpClient.Dispose();
        }
    }
}
