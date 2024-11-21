"use strict";

var mainConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

// Receive message
mainConnection.on("ReceiveMessage", function (user, message) {
    var msg = sanitizeMessage(message);
    var time = getCurrentTime();

    var encodedMsg = `
        <div class='border border-primary rounded-4 m-1 p-1'>
            <p><strong>${user}</strong> <span class='messageTime'>${time}</span><hr></p>
            <p>${msg}</p>
        </div>`;
    appendMessage("messages", encodedMsg);
});

// Chat hub connection established
mainConnection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    sendBotMessage("Hello! Please feel free to use this Gemini chat bot to flush out your project requirements.");
}).catch(handleError);

// Send button click event
document.getElementById("sendButton").addEventListener("click", function (event) {
    handleSendMessage(mainConnection, "messageInput", "messages");
    event.preventDefault();
});

// Key up event to trigger send on Enter
document.getElementById("messageInput").addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        document.getElementById("sendButton").click();
    }
    event.preventDefault();
});

// Utility functions
function sanitizeMessage(message) {
    return message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
}

function getCurrentTime() {
    var today = new Date();
    return today.getMinutes() < 10 ? `${today.getHours()}:0${today.getMinutes()}` : `${today.getHours()}:${today.getMinutes()}`;
}

function appendMessage(containerId, message) {
    var element = document.createElement("div");
    element.innerHTML = message;
    document.getElementById(containerId).appendChild(element);
}

function sendBotMessage(botMessage) {
    var time = getCurrentTime();
    var encodedMsg = `
        <div class='border border-primary rounded-4 m-1 p-1'>
            <p><strong>Chat Bot</strong> <span class='messageTime'>${time}</span><hr></p>
            <p>${botMessage}</p>
        </div>`;
    appendMessage("messages", encodedMsg);
}

function handleSendMessage(connection, inputId, containerId) {
    var message = document.getElementById(inputId).value;
    if (message.trim() !== "") {
        connection.invoke("SendMessage", message).catch(handleError);
        document.getElementById(inputId).value = "";
    } else {
        sendBotMessage("Message cannot be blank.");
    }
}

function handleError(err) {
    console.error(err.toString());
}
