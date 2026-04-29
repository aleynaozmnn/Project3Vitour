using MailKit.Net.Smtp;
using MimeKit;
namespace Project3Vitour.Services.MailServices
{
    public class MailService : IMailService
    {
        public void SendMail(string receiverMail, string subject, string body)
        {
            var mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressFrom = new MailboxAddress("Vitour Admin","aleynaozmenn629@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", receiverMail);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder=new BodyBuilder();
            bodyBuilder.TextBody = body;
            mimeMessage.Body=bodyBuilder.ToMessageBody();
            mimeMessage.Subject=subject;
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("aleynaozmenn629@gmail.com", "n d j f l e v z s h c m m b k p");
                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }
    }
}
