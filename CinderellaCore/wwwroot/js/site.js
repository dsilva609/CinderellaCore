// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function askForPermissionToReceiveNotifications() {
	try {
		const messaging = firebase.messaging();
		await messaging.requestPermission().then(function() {
			console.log("permission granted");
		}).catch(function(err) {
			console.log("permission failed", err);
		});
		const token = messaging.getToken().then(function(token) {
			console.log('token:', token);
		});
		

		return token;
	} catch (error) {
		console.error("error " + error);
	}
}