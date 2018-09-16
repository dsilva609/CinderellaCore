async function askForPermissionToReceiveNotifications() {
    try {
        const messaging = firebase.messaging();
        await messaging.requestPermission().then(function () {
            console.log("permission granted");
        }).catch(function (err) {
            console.log("permission failed", err);
        });
        const token = messaging.getToken().then(function (token) {
            console.log('token:', token);
        });

        return token;
    } catch (error) {
        console.error("error " + error);
    }
}