$(document).ready(function() {
    $("#TestAPIBtn").click(function(){
        let message = $("#Description").val();

        if (message === "" || message === undefined) {
            //Check for empty string before sending to API
            $("#GemeniResponse").text("Please say something before calling on gemeni");
        }
        else {
            //Call the FastAPI Function
            SendMessageGemini(message);
        }
    }); 
});
function SendMessageGemini(message) {
    fetch("http://127.0.0.1:8000/genResponse?text=" + message)
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

                document.getElementById("experimentalChat").appendChild(experimentalElement);
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            //What happens when the response is returned successfully from FastAPI Server
            var today = new Date();
            if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
            else { var time = today.getHours() + ":" + today.getMinutes(); }

            var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p style='white-space: pre-line'>    " + data + "</p></div>";
            var element = document.createElement("div");
            element.innerHTML = encodedMsg;
            document.getElementById("messages").appendChild(element);

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

                if (projectDetails.Requirements.length > 0) {
                    projectDetails.Requirements.forEach(function (item, index) {
                        document.getElementById("Description").value += "\n"; 
                        document.getElementById("Description").value += item;
                    });
                }


                //Append each tech stack to description!
                document.getElementById("Description").value += "\n\n"; 
                document.getElementById("Description").value += "Technologies Required";

                if (projectDetails['Technologies Required'].length > 0 && Array.isArray(projectDetails['Technologies Required'])) {
                    projectDetails['Technologies Required'].forEach(function (item, index) {
                        document.getElementById("Description").value += "\n";
                        document.getElementById("Description").value += item;
                    });
                }
                else {
                    document.getElementById("Description").value += "\n";
                    document.getElementById("Description").value += projectDetails['Technologies Required'];
                }


                var encodedMsg = "<div class=' rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p style='white-space: pre-line' class='text-light'>    " + summarizedProjectJson2[2] + "</p></div>";
            }
            else {
                var encodedMsg = "<div class=' rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p style='white-space: pre-line' class='text-light'>    " + data + "</p></div>";
            }

            var experimentalElement = element.cloneNode(true);
            experimentalElement.innerHTML = encodedMsg;
            experimentalElement.classList.add("row");

            if (data.length > 0) {
                document.getElementById("experimentalChat").appendChild(experimentalElement);
            }
        })
        .catch(error => {
            var today = new Date();
            if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
            else { var time = today.getHours() + ":" + today.getMinutes(); }

            var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "    I could not process the message due to "+ error +". Please try again." + "</p></div>";
            var element = document.createElement("div");
            element.innerHTML = encodedMsg;
            document.getElementById("messages").appendChild(element);

            var encodedMsg = "<div class='rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "    I could not process the message due to "+ error +". Please try again." + "</p></div>";
            var experimentalElement = element.cloneNode(true);
            experimentalElement.innerHTML = encodedMsg;
            experimentalElement.classList.add("row");
            document.getElementById("experimentalChat").appendChild(experimentalElement);
            console.error('Fetch error:', error);
        })
}




