namespace SportsStore.Domain.Abstract
{

    public interface IEmailSender
    {
        void SendMessage(string email,string subjet, string body);
    }

}
