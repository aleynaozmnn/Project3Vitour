namespace Project3Vitour.Services.MailServices
{
    public interface IMailService
    {
        void SendMail(string receiverMail, string subject, string body);
    }
}
