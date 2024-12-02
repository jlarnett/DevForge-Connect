$(document).ready(function () {
    $("#sendButton").click(function () {
        let message = $("#messageInput").val();

        if (message === "" || message === undefined) {
            // Check for empty message before sending to API
            $("#messages").append("<div class='border border-danger rounded-4 m-1 p-1'><p><strong>Chat Bot</strong><hr>Please enter a message before sending.</p></div>");
        } else {
            // Call the FastAPI function to send the message
            SendMessageGemini(message);
        }
    });
});

function SendMessageGemini(message) {
    var loadingMsg = "<div class='border border-primary rounded-4 m-1 p-1' id='loadingMessage'><p><strong>Chat Bot</strong><span class='messageTime'></span><hr>Processing your message...</p></div>";
    var element = document.createElement("div");
    element.innerHTML = loadingMsg;
    document.getElementById("messages").appendChild(element);

    var loadingSpinner = "<div id='load' class='spinner-border' role='status'><span class='sr-only'></span></div>";
    var experimentalElement = document.createElement("div");
    experimentalElement.innerHTML = loadingSpinner;
    experimentalElement.classList.add("row");
    document.getElementById("myForm").appendChild(experimentalElement);

    const chatContainer = document.getElementById("messages");
    chatContainer.scrollTop = chatContainer.scrollHeight;

    fetch("http://127.0.0.1:8000/genResponse?text=" + encodeURIComponent(message))
        .then(response => {
            if (!response.ok) {
                document.getElementById("loadingMessage").remove();
                document.getElementById("load").remove();
                var errorMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>Chat Bot</strong><span class='messageTime'></span><hr>I could not process the message. Please try again.</p></div>";
                var errorElement = document.createElement("div");
                errorElement.innerHTML = errorMsg;
                document.getElementById("messages").appendChild(errorElement);
                const chatContainer = document.getElementById("messages");
                chatContainer.scrollTop = chatContainer.scrollHeight;
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            document.getElementById("loadingMessage").remove();
            document.getElementById("load").remove();

            var today = new Date();
            var time = today.getHours() + ":" + (today.getMinutes() < 9 ? "0" : "") + today.getMinutes();

            var botMessage = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>Chat Bot</strong><span class='messageTime'>" + time + "</span><hr>" + data + "</p></div>";
            var botElement = document.createElement("div");
            botElement.innerHTML = botMessage;
            document.getElementById("messages").appendChild(botElement);
            const chatContainer = document.getElementById("messages");
            chatContainer.scrollTop = chatContainer.scrollHeight;

            // If the response contains project details in markdown format (e.g., JSON inside triple backticks)
            const summarizedProjectJson2 = data.split("```");

            if (summarizedProjectJson2.length > 1) {
                let summarizedProjectJson = summarizedProjectJson2[1].slice(4);
                let projectDetails = JSON.parse(summarizedProjectJson);

                // You can handle the project details here, for example:
                console.log(projectDetails);

                document.getElementById("Title").value = projectDetails.Title;
                document.getElementById("Description").value = projectDetails.Description;
                document.getElementById("Funding").value = projectDetails.Funding;

                // Append requirements to the description
                document.getElementById("Description").value += "\n\nRequirements\n" + projectDetails.Requirements;

                // Append technologies to the description
                document.getElementById("Description").value += "\n\nTechnologies Required\n" + projectDetails.TechnologiesRequired;
            }

        })
        .catch(error => {
            document.getElementById("loadingMessage").remove();
            document.getElementById("load").remove();

            var today = new Date();
            var time = today.getHours() + ":" + (today.getMinutes() < 9 ? "0" : "") + today.getMinutes();

            var errorMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>Chat Bot</strong><span class='messageTime'>" + time + "</span><hr>Could not process the message due to " + error + ". Please try again.</p></div>";
            var errorElement = document.createElement("div");
            errorElement.innerHTML = errorMsg;
            document.getElementById("messages").appendChild(errorElement);
            const chatContainer = document.getElementById("messages");
            chatContainer.scrollTop = chatContainer.scrollHeight;
            console.error('Fetch error:', error);
        });
}
