

namespace BestBuy.Core.Notification
{
    public interface INotificationContext
    {
        bool HasNotifications();
        int Code { get; }
        IReadOnlyCollection<string> GetNotifications();
        void AddNotification(int code, string message);
        void AddNotifications(IEnumerable<string> notifications);
        void AddNotification(int code, object message);
        void CleanNotifications(int code);
        void CleanNotifications();
    }
}
