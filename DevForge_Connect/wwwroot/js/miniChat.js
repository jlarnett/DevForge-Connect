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

$('#projectform').on('submit', function () {

    return true;
});

const formBtn = document.getElementById('SubmitAIProjectButton');

formBtn.addEventListener("click", (event) => {
    event.preventDefault();
    fetch("http://127.0.0.1:8000/genResponse?text=" + "summarize project")
        .then(response => {
            if (!response.ok) {
                //When we receive a not ok network response from FastAPI Server
                var today = new Date();
                if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
                else { var time = today.getHours() + ":" + today.getMinutes(); }

                var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "    I could not process the message. Please try again." + "</p></div>";
                var element = document.createElement("div");
                element.innerHTML = encodedMsg;
                document.getElementById("messages").appendChild(element);


                var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "    I could not process the message. Please try again." + "</p></div>";
                var experimentalElement = element.cloneNode(true);
                experimentalElement.classList.add("row");
                experimentalElement.innerHTML = encodedMsg;
            }
            return response.json();
        })
        .then(data => {
            const summarizedProjectJson2 = data.split("```");

            if (summarizedProjectJson2.length > 1) {
                let summarizedProjectJson = summarizedProjectJson2[1].slice(4);
                let projectDetails = JSON.parse(summarizedProjectJson);

                console.log(projectDetails);
                document.getElementById("Title").value = projectDetails.Title;
                document.getElementById("Description").value = projectDetails.Description;
                document.getElementById("Funding").value = projectDetails.Funding;

                //Append each requirement to description!
                document.getElementById("Description").value += "\n\n";
                document.getElementById("Description").value += "Requirements";

                document.getElementById("Description").value += "\n";
                document.getElementById("Description").value += projectDetails.Requirements;

                //Append each tech stack to description!
                document.getElementById("Description").value += "\n\n";
                document.getElementById("Description").value += "Technologies Required";

                document.getElementById("Description").value += "\n";
                document.getElementById("Description").value += projectDetails.TechnologiesRequired;

                document.getElementById('projectform').submit();

            }
        });
});