importScripts('https://www.gstatic.com/firebasejs/5.4.2/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/5.4.2/firebase-messaging.js');

var config = {
	apiKey: "AIzaSyD2hPRsVJUdq4MPQOCHwhRq5aocQq0i4Ss",
	authDomain: "project-cinderella.firebaseapp.com",
	databaseURL: "https://project-cinderella.firebaseio.com",
	projectId: "project-cinderella",
	storageBucket: "project-cinderella.appspot.com",
	messagingSenderId: "180792182839"
};
firebase.initializeApp(config);
const messaging = firebase.messaging(); 
messaging.setBackgroundMessageHandler(function (payload) {
	new Notification("test");
//	var data = JSON.parse(payload);
//	self.registration.showNotification(data.title, {
//		body: data.body,
//		icon: data.icon,
//		click_action: data.click_action,
//		//time_to_live: data.time_to_live,
//		//data: data.data,
//		//tag: data.tag
//	});
//	console.log("received");
});
//firebase.messaging().onMessage(function (payload) {
//	console.log('Message received. ', payload);
//	//firebase.notifications().displayNotification(payload);
//});
