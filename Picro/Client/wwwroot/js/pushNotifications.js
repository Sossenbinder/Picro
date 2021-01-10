(function () {
    // Note: Replace with your own key pair before deploying
    const applicationServerPublicKey = 'BLXfJOjUknl_JijpS5xzTvvkcOlSytIX3Jwp_ztSPK1ujOVf8KeBnqaeriAvX5Y0Z0b9j88vKFi65OAQCmeI0qg';

    window.notification = {
        requestSubscription: async () => {
            const worker = await navigator.serviceWorker.getRegistration();

            const existingSubscription = await worker.pushManager.getSubscription();

            if (!existingSubscription) {
                const newSubscription = await subscribe(worker);
                if (newSubscription) {
                    return {
                        url: newSubscription.endpoint,
                        p256dh: arrayBufferToBase64(newSubscription.getKey('p256dh')),
                        auth: arrayBufferToBase64(newSubscription.getKey('auth'))
                    };
                }
            }
        },
        showNotification: async (title) => {
            const serviceWorker = await navigator.serviceWorker.getRegistration();
            serviceWorker.showNotification('Blazing Pizza',
                {
                    body: "Blaaaa",
                    icon: 'img/icon-512.png',
                    vibrate: [100, 50, 100],
                    data: { url: "localhost" }
                });
        },
    };

    const subscribe = async (worker) => {
        try {
            return await worker.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: applicationServerPublicKey
            });
        } catch (error) {
            if (error.name === 'NotAllowedError') {
                return null;
            }
            throw error;
        }
    };

    function arrayBufferToBase64(buffer) {
        // https://stackoverflow.com/a/9458996
        let binary = '';
        const bytes = new Uint8Array(buffer);
        const len = bytes.byteLength;
        for (let i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    }
})();