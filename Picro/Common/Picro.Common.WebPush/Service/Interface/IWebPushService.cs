using System.Threading.Tasks;

namespace Picro.Common.WebPush.Service.Interface
{
    public interface IWebPushService
    {
        Task SendNotificationToSession();
    }
}