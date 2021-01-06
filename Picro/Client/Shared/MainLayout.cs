using Microsoft.AspNetCore.Components;
using Picro.Client.Services.Interface;
using Picro.Client.Utils;

namespace Picro.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private GlobalClickHandler GlobalClickHandler { get; set; }

        [Inject]
        private INotificationsService NotificationsService { get; set; }

        protected override void OnInitialized()
        {
            // Do not await this => Should happen in the background
            _ = NotificationsService.RequestNotificationSubscription();
        }
    }
}