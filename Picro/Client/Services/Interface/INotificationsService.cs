using System.Threading.Tasks;

namespace Picro.Client.Services.Interface
{
	public interface INotificationsService
	{
		Task RequestNotificationSubscription();
	}
}