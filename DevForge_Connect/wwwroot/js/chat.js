﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

// Receive message
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var today = new Date();
    if (today.getMinutes() < 10) { var time = today.getHours() + ":0" + today.getMinutes(); }
    else {var time = today.getHours() + ":" + today.getMinutes(); }
    var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>    " + user + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + msg + "</p></div>";
    var element = document.createElement("div");
    element.innerHTML = encodedMsg;
    document.getElementById("messages").appendChild(element);

    //Experimental ChatBot Window Located next to Project Submission Form
    //Just seperating out the different UIs so they don't mess with each other
    var encodedMsg = "<div class='rounded-4 m-1 p-1 text-end'><p><strong class='text-light'>    " + user + "</strong> <span class='messageTime text-light'>" + time + "</span><hr></p><p class='text-light'>    " + msg + "</p></div>";
    var experimentalElement = element.cloneNode(true);
    experimentalElement.classList.add("row");
    experimentalElement.innerHTML = encodedMsg;
    document.getElementById("experimentalChat").appendChild(experimentalElement);
    const chatContainer = document.getElementById("experimentalChat");
    chatContainer.scrollTop = chatContainer.scrollHeight;
});

// Chat hub connection established
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    var today = new Date();
    if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
    else { var time = today.getHours() + ":" + today.getMinutes(); }

    var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "Hello! Please feel free to use this Gemeni chat bot to flush out your project requirements." + "</p></div>";
    var element = document.createElement("div");
    element.innerHTML = encodedMsg;
    document.getElementById("messages").appendChild(element);

    //Experimental ChatBot Window Located next to Project Submission Form
    //Just seperating out the different UIs so they don't mess with each other
    var encodedMsg = "<div class='rounded-4 m-1 p-1'><p><strong class='text-light'>" + "    Chat Bot" + "</strong> <span class='messageTime text-light'>" + time + "</span><hr></p><p class='text-light'>    " + "Hello! Please feel free to use this Gemeni chat bot to flush out your project requirements." + "</p></div>";
    var experimentalElement = element.cloneNode(true);
    experimentalElement.classList.add("row");
    experimentalElement.innerHTML = encodedMsg;
    document.getElementById("experimentalChat").appendChild(experimentalElement);
}).catch(function (err) {
    return console.error(err.toString());
});

// Send button click event
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;

    if (message !== "") {
        const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

        (async () => {
            try {
                await connection.invoke("SendMessage", message); 
                SendMessageGemini(msg); 
            } catch (err) {
                console.error(err.toString());
            }
        })();
    }
    else {
        var today = new Date();
        if (today.getMinutes() < 10) { var time = today.getHours() + ":0" + today.getMinutes(); }
        else { var time = today.getHours() + ":" + today.getMinutes(); }
        
        var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "Message can not be blank." + "</p></div>";
        var element = document.createElement("div");
        element.innerHTML = encodedMsg;
        document.getElementById("messages").appendChild(element);

        
        //Experimental ChatBot Window Located next to Project Submission Form
        //Just seperating out the different UIs so they don't mess with each other
        var encodedMsg = "<div class='border border-light rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "Message can not be blank." + "</p></div>";
       var experimentalElement = element.cloneNode(true);
        experimentalElement.innerHTML = encodedMsg;
        experimentalElement.classList.add("row");
        document.getElementById("experimentalChat").appendChild(experimentalElement);
        const chatContainer = document.getElementById("experimentalChat");
        chatContainer.scrollTop = chatContainer.scrollHeight;
    }

    document.getElementById("messageInput").value = "";
    event.preventDefault();
});

// Send button click event
document.getElementById("ExperimentalSendChat").addEventListener("click", function (event) {
    var message = document.getElementById("ExperimentalInput").value;

    if (message.trim() !== "") {
        const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
   
        (async () => {
            try {
                await connection.invoke("SendMessage", message); 
                SendMessageGemini(msg); 
            } catch (err) {
                console.error(err.toString());
            }
        })();
    }
    else {
        var today = new Date();
        if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
        else { var time = today.getHours() + ":" + today.getMinutes(); }
        
        var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "Message can not be blank or is too short." + "</p></div>";
        var element = document.createElement("div");
        element.innerHTML = encodedMsg;
        document.getElementById("messages").appendChild(element);

        
        //Experimental ChatBot Window Located next to Project Submission Form
        //Just seperating out the different UIs so they don't mess with each other
        var encodedMsg = "<div class='border border-light rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "Message can not be blank or is too short." + "</p></div>";
        var experimentalElement = element.cloneNode(true);
        experimentalElement.innerHTML = encodedMsg;
        experimentalElement.classList.add("row");
        document.getElementById("experimentalChat").appendChild(experimentalElement);
        const chatContainer = document.getElementById("experimentalChat");
        chatContainer.scrollTop = chatContainer.scrollHeight;
    }

    document.getElementById("ExperimentalInput").value = "";
    event.preventDefault();
});



// Message input field key up event
document.getElementById("messageInput").addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        document.getElementById("sendButton").click();
    }
    event.preventDefault();
});

document.getElementById("ExperimentalInput").addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        document.getElementById("ExperimentalSendChat").click();
    }
    event.preventDefault();
});
