To test about SignalR Run following code :

Step 1 : Load SignalR library in console .


var script = document.createElement("script");
script.src = "https://cdn.jsdelivr.net/npm/@microsoft/signalr@7.0.5/dist/browser/signalr.min.js";
document.head.appendChild(script);

Step 2: Run following code also add access token 


const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7004/hubs/notifications", {
        accessTokenFactory: () => "YOUR_TOKEN"
    })
    .withAutomaticReconnect()
    .build();

connection.on("ReceiveNotification", (msg) => {
    console.log("🔔 Notification:", msg);
});

connection.start()
    .then(() => console.log(" Connected to SignalR Hub"))
    .catch(err => console.error(" SignalR connection error:", err));