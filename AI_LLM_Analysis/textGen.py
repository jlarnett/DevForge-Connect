import google.generativeai as genai

import os

#Configure this to be run from a environment variable
genai.configure(api_key=os.environ["GEMENI_API_KEY"])



model = genai.GenerativeModel(
    model_name="gemini-1.5-flash",
    system_instruction="You are a helpful assistant, who is able to identify key requirements in a requirment gathering process. Please keep you're answers as concise as possible.")
# response = model.generate_content("Write a story about a magic backpack.")
# print(response.text)

chat = model.start_chat(
    history=[
        {"role": "user", "parts": "Hello, I'm going to be asking questions about projects and I need help flushing out the requirements. Please keep answers as concise as possible"},
        {"role": "model", "parts": "Great to meet you. What would you like to know?"},
    ]
)

def generate_response(text):
    response = chat.send_message(text)
    return response.text


# response = chat.send_message("I want to build a project that has the ability users to communicate with each other.  Additionally I need to be able to keep their saved information")
# print("RESPONSE 1", response.text)
# response = chat.send_message("I now want to be able to create a user friend interface")
# print("RESPOSNE 2", response.text)