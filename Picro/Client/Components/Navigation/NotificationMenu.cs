using Microsoft.AspNetCore.Components;
using Picro.Client.Services.Interface;

namespace Picro.Client.Components.Navigation
{
    public partial class NotificationMenu : ComponentBase
    {
        [Inject]
        private IDistributedImageService DistributedImageService { get; set; }

        private int _notificationCount;

        private bool _isOpened;

        protected override void OnInitialized()
        {
            _notificationCount = 0;

            DistributedImageService.ImageReceived.Register(OnNewImageReceived);
        }

        private void OnNewImageReceived()
        {
            _notificationCount++;
            StateHasChanged();
        }

        private void OnBellClick()
        {
            if (_isOpened)
            {
                _notificationCount = 0;
            }

            _isOpened = !_isOpened;
        }
    }
}