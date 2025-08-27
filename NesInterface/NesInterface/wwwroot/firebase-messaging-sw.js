importScripts('https://www.gstatic.com/firebasejs/9.23.0/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/9.23.0/firebase-messaging-compat.js');

const firebaseConfig = {
    apiKey: "AIzaSyBneZL0iDCWU0jetLMujGapvKcBzojG9Dg",
    authDomain: "nesservice-6687d.firebaseapp.com",
    projectId: "nesservice-6687d",
    storageBucket: "nesservice-6687d.firebasestorage.app",
    messagingSenderId: "453480690657",
    appId: "1:453480690657:web:49b7b971171c73e3d698a0",
    measurementId: "G-YJG6C1FPH4"
};

firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
    console.log('Background mesaj alındı:', payload);
});

self.addEventListener('install', (event) => {
    console.log('Service Worker installing...');
    self.skipWaiting();
});

self.addEventListener('message', (event) => {
    if (event.data && event.data.type === 'SKIP_WAITING') {
        console.log('SKIP_WAITING mesajı alındı');
        self.skipWaiting();
    }
});

self.addEventListener('activate', (event) => {
    console.log('Service Worker activating...');
    event.waitUntil(self.clients.claim());
});