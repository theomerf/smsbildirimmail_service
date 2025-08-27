
import { initializeApp } from 'https://www.gstatic.com/firebasejs/9.23.0/firebase-app.js';
import { getMessaging, getToken, onMessage } from 'https://www.gstatic.com/firebasejs/9.23.0/firebase-messaging.js';

const firebaseConfig = {
    apiKey: "AIzaSyBneZL0iDCWU0jetLMujGapvKcBzojG9Dg",
    authDomain: "nesservice-6687d.firebaseapp.com",
    projectId: "nesservice-6687d",
    storageBucket: "nesservice-6687d.firebasestorage.app",
    messagingSenderId: "453480690657",
    appId: "1:453480690657:web:49b7b971171c73e3d698a0",
    measurementId: "G-YJG6C1FPH4"
};

const app = initializeApp(firebaseConfig);
const messaging = getMessaging(app);

const statusDiv = document.getElementById('status');
const tokenDiv = document.getElementById('token');

function updateStatus(message) {
    statusDiv.innerHTML += '<p>' + new Date().toLocaleTimeString() + ': ' + message + '</p>';
    console.log(message);
}

async function registerServiceWorker() {
    if ('serviceWorker' in navigator) {
        try {
            const registration = await navigator.serviceWorker.register('/firebase-messaging-sw.js');
            updateStatus('Service Worker kayıt edildi');

            if (registration.installing) {
                updateStatus('Service Worker installing...');
                await new Promise(resolve => {
                    registration.installing.addEventListener('statechange', () => {
                        if (registration.installing.state === 'installed') {
                            resolve();
                        }
                    });
                });
            }

            if (registration.waiting) {
                updateStatus('Service Worker waiting...');
                registration.waiting.postMessage({ type: 'SKIP_WAITING' });
                await new Promise(resolve => {
                    navigator.serviceWorker.addEventListener('controllerchange', resolve, { once: true });
                });
            }

            if (!registration.active) {
                updateStatus('Service Worker aktif değil, bekleniyor...');
                await new Promise(resolve => {
                    const checkState = () => {
                        if (registration.active) {
                            resolve();
                        } else {
                            setTimeout(checkState, 100);
                        }
                    };
                    checkState();
                });
            }

            updateStatus('Service Worker aktif ve hazır');
            return registration;
        } catch (error) {
            updateStatus('Service Worker kayıt hatası: ' + error.message);
            throw error;
        }
    } else {
        throw new Error('Service Worker desteklenmiyor');
    }
}

async function initializeNotifications() {
    try {
        updateStatus('Bildirim başlatılıyor...');

        const registration = await registerServiceWorker();

        await navigator.serviceWorker.ready;
        updateStatus('Service Worker tamamen hazır');

        await new Promise(resolve => setTimeout(resolve, 500));

        const permission = await Notification.requestPermission();

        if (permission === 'granted') {
            updateStatus('Bildirim izni verildi');

            try {
                let token = null;
                let attempts = 0;
                const maxAttempts = 3;

                while (!token && attempts < maxAttempts) {
                    attempts++;
                    updateStatus(`Token alma denemesi ${attempts}/${maxAttempts}`);

                    try {
                        token = await getToken(messaging, {
                            vapidKey: 'BENuhrbglkd32Cg61deKwQlXKNYZFEDpuaksNOExynUjeIEoS-stK71jziNsFRxXI6SGSdN-bT2iTELBzOVz5jg',
                            serviceWorkerRegistration: registration
                        });

                        if (token) {
                            break;
                        }
                    } catch (tokenError) {
                        updateStatus(`Token alma hatası (deneme ${attempts}): ${tokenError.message}`);
                        if (attempts < maxAttempts) {
                            await new Promise(resolve => setTimeout(resolve, 1000));
                        }
                    }
                }

                if (token) {
                    updateStatus('FCM Token başarıyla alındı');
                    tokenDiv.innerHTML = '<strong>Token:</strong><br><textarea style="width:100%; height:100px;">' + token + '</textarea>';

                    await sendTokenToServer(token);

                    setupForegroundListener();

                } else {
                    updateStatus('Token alınamadı - VAPID key veya Service Worker sorunu olabilir');
                }
            } catch (tokenError) {
                updateStatus('Token alma sürecinde hata: ' + tokenError.message);
                console.error('Token error details:', tokenError);
            }
        } else {
            updateStatus('Bildirim izni reddedildi: ' + permission);
        }
    } catch (error) {
        updateStatus('Initialization hatası: ' + error.message);
        console.error('Initialization error details:', error);
    }
}

function setupForegroundListener() {
    onMessage(messaging, (payload) => {
        updateStatus('Foreground mesaj alındı');
        console.log('Foreground mesaj:', payload);

        const title = payload.notification?.title || 'Yeni Bildirim';
        const body = payload.notification?.body || 'Yeni bir mesajınız var';

        if (Notification.permission === 'granted') {
            new Notification(title, {
                body: body,
                icon: payload.notification?.icon || '/icon-192x192.png'
            });
        }

        showInPageNotification(title, body);
    });
}

async function sendTokenToServer(token) {
    try {
        // Simülasyon
        updateStatus('Token sunucuya gönderildi (simülasyon)');
    } catch (error) {
        updateStatus('Token gönderim hatası: ' + error.message);
    }
}

function showInPageNotification(title, body) {
    const notification = document.createElement('div');
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #4CAF50;
        color: white;
        padding: 15px;
        border-radius: 5px;
        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        z-index: 10000;
        max-width: 300px;
        `;
    notification.innerHTML = `<strong>${title}</strong><br>${body}`;
    document.body.appendChild(notification);

    setTimeout(() => {
        if (notification.parentNode) {
            notification.parentNode.removeChild(notification);
        }
    }, 5000);
}

function sendTestNotification() {
    if (Notification.permission === 'granted') {
        new Notification('Test Bildirimi', {
            body: 'Bu bir test bildirimidir',
            icon: '/icon-192x192.png'
        });
        updateStatus('Test bildirimi gönderildi');
    } else {
        updateStatus('Bildirim izni gerekli');
    }
}

document.getElementById('initNotifications').addEventListener('click', initializeNotifications);
document.getElementById('sendTest').addEventListener('click', sendTestNotification);

updateStatus('Sayfa yüklendi - Firebase FCM hazır');


if ('serviceWorker' in navigator) {
    navigator.serviceWorker.ready.then(() => {
        updateStatus('Service Worker hazır');
    });
}
