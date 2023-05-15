using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBestBuy.Api.Controllers
{
    public class BaseController : ControllerBase
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


        protected string CreateCartId()
        {
            var cart = HttpContext.Request.Cookies["CartId"];

            if (string.IsNullOrEmpty(cart))
            {
                var id = Guid.NewGuid();


                HttpContext.Response.Cookies.Append("CartId", id.ToString());
                return id.ToString();
            }

            return cart;

        }
    }
}

