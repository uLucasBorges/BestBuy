using BestBuy.Core.Notification;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBestBuy.Controllers
{
    public class BaseController : Controller
    {
        protected readonly INotificationContext _notificationContext;
        protected BaseController(INotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        protected bool IsValidOperation()
        {
            return !_notificationContext.HasNotifications();
        }

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(result);
            }

            var notifications = _notificationContext.GetNotifications();

            return StatusCode(_notificationContext.Code, notifications);
        }
    }
}
