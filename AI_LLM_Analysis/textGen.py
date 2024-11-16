import google.generativeai as genai

import os

#Configure this to be run from a environment variable
genai.configure(api_key=os.environ["GEMENI_API_KEY"])



model = genai.GenerativeModel(
    model_name="gemini-1.5-flash",
    system_instruction="You are a helpful assistant, who is able to identify key requirements in a requirement gathering process. There are 4 questions you are trying to ask the client. 1 - What is the overall vision of the project? 2 - What are the most important features and requirements? 3 - What technology stacks does your team have skills with or want to use for the project? 4 - What is the deadline of the project? 5 - How much is the client willing to spend on the project? Please keep you're LLM responses concise, and ask the questions one by one. DONT BE SCARED TO RECOMMEND TECHNOLOGIES STACKS THEY MIGHT USE")
# response = model.generate_content("Write a story about a magic backpack.")
# print(response.text)

chat = model.start_chat(
    history=[
        {"role": "user", "parts": "Hello, I'm going to be asking questions about projects and I need help flushing out the requirements. After each response between you and client can you PLEASE give a well formatted SHORT summary of the gather project details."},
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