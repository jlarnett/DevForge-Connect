import google.generativeai as genai
from dotenv import load_dotenv
import os

# Load environment variables from the .env file
load_dotenv()

# Now you can access the API key directly from the environment variables
genai.configure(api_key=os.getenv("GEMENI_API_KEY"))



model = genai.GenerativeModel(
    model_name="gemini-1.5-flash",
    system_instruction="You are a helpful assistant, who is able to identify key requirements in a requirement gathering process. There are 4 questions you are trying to ask the client 1 - What is the overall vision of the project? 2 - What are the most important features and requirements? 3 - What technology stacks does your team have skills with or want to use for the project? 4 - What is the deadline of the project? 5 - How much is the client willing to spend on the project?. If the user doesn't kow the answer recommend potential answers, but ask the questions 1 per response")
# response = model.generate_content("Write a story about a magic backpack.")
# print(response.text)

chat = model.start_chat(
    history=[
        {"role": "user", "parts": "Hello, I'm going to be asking questions about projects and I need help flushing out the requirements."},
        {"role": "user", "parts": "If the user doesn't give you a response recommend a tech stack or possible answers to the question"},
        {"role": "user", "parts": "If I send you 'summarize project' return a json formatted summary. I want the json to include Title, Description, Requirements,deadline, funding, and TechnologiesRequired to be included. The json SHOULD NOT include nested arrays or keys"},
        {"role": "model", "parts": "Great to meet you! Please give me some details about the vision of your project? What are some of the features you would like to see?"},
    ]
)

def generate_response(text):
    response = chat.send_message(text)
    return response.text


# response = chat.send_message("I want to build a project that has the ability users to communicate with each other.  Additionally I need to be able to keep their saved information")
# print("RESPONSE 1", response.text)
# response = chat.send_message("I now want to be able to create a user friend interface")
# print("RESPOSNE 2", response.text)