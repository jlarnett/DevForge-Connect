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

        })
        .catch(error => {
            var today = new Date();
            if (today.getMinutes() < 9) { var time = today.getHours() + ":0" + today.getMinutes(); }
            else { var time = today.getHours() + ":" + today.getMinutes(); }

            var encodedMsg = "<div class='border border-primary rounded-4 m-1 p-1'><p><strong>" + "    Chat Bot" + "</strong> <span class='messageTime'>" + time + "</span><hr></p><p>    " + "    I could not process the message due to "+ error +". Please try again." + "</p></div>";
            var element = document.createElement("div");
            element.innerHTML = encodedMsg;
            document.getElementById("messages").appendChild(element);

            console.error('Fetch error:', error);
        })
}




