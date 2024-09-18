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
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json();
        })
        .then(data => {
            //What happens when the response is returned successfully from FastAPI Server
            $("#GemeniResponse").text(data);
        })
        .catch(error => console.error('Fetch error:', error));
}




