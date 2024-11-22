"use strict";

var miniConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable send button until connection is established
document.getElementById("ExperimentalSendChat").disabled = true;

// Receive message
miniConnection.on("ReceiveMessage", function (user, message) {
    var msg = sanitizeMessage(message);
    var time = getCurrentTime();

    var encodedMsg = `
        <div class='border border-light rounded-4 m-1 p-1'>
            <p><strong>${user}</strong> <span class='messageTime'>${time}</span><hr></p>
            <p>${msg}</p>
        </div>`;
    appendMessage("experimentalChat", encodedMsg);
});

// Chat hub connection established
miniConnection.start().then(function () {
    document.getElementById("ExperimentalSendChat").disabled = false;
    sendBotMessage("Hello! Feel free to ask any questions in this chat!");
}).catch(handleError);

// Send button click event
document.getElementById("ExperimentalSendChat").addEventListener("click", function (event) {
    handleSendMessage(miniConnection, "ExperimentalInput", "experimentalChat");
    event.preventDefault();
});

// Key up event to trigger send on Enter
document.getElementById("ExperimentalInput").addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        document.getElementById("ExperimentalSendChat").click();
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
        <div class='border border-light rounded-4 m-1 p-1'>
            <p><strong>Chat Bot</strong> <span class='messageTime'>${time}</span><hr></p>
            <p>${botMessage}</p>
        </div>`;
    appendMessage("experimentalChat", encodedMsg);
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
