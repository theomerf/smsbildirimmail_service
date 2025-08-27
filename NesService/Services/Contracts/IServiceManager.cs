namespace Services.Contracts
{
    public interface IServiceManager
    {
        IMailService MailService { get; }
        INotificationService NotificationService { get; }
        ISmsService SmsService { get; }
    }
}
