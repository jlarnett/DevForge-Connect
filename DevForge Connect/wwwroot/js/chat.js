"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

// Receive message
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var today = new Date();
    if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
    else {var time = today.getHours() + ":" + today.getMinutes(); }
    
    var encodedMsg = "<div class='border border-primary rounded-4 m-1'><p><strong>    " + user + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + msg + "</p></div>";
    var element = document.createElement("div");
    element.innerHTML = encodedMsg;
    document.getElementById("messages").appendChild(element);
});

// Chat hub connection established
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

// Send button click event
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;

    if (message.trim() !== "") {
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        SendMessageGemini(msg);
    }
    else {
        var today = new Date();
        if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
        else { var time = today.getHours() + ":" + today.getMinutes(); }

        var encodedMsg = "<div class='border border-primary rounded-4 m-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "Message can not be blank." + "</p></div>";
        var element = document.createElement("div");
        element.innerHTML = encodedMsg;
        document.getElementById("messages").appendChild(element);
    }

    document.getElementById("messageInput").value = "";
    event.preventDefault();
});



// Message input field key up event
document.getElementById("messageInput").addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        document.getElementById("sendButton").click();
    }
    event.preventDefault();
});
