import spacy
from spacy.training import Example
import random
import json

# Create a blank 'en' model
nlp = spacy.blank("en")

# Add the text categorizer to the pipeline
# Ensure we only add it once
if "textcat" not in nlp.pipe_names:
    textcat = nlp.add_pipe("textcat", last=True)

# Add labels for the text categorizer
textcat.add_label("STORAGE")
textcat.add_label("AUTHENTICATION")
textcat.add_label("DATA_ANALYSIS")
textcat.add_label("COMMUNICATION")
textcat.add_label("WEB_DEVELOPMENT")

# Initialize the model after adding the textcat and labels
nlp.initialize()

# Example training data
# TRAINING_DATA = [
#     ("I want to build an app that stores customer recipes to be accessed by everyone.", {"cats": {"STORAGE": 1, "AUTHENTICATION": 0, "DATA_ANALYSIS": 0, "COMMUNICATION": 0, "WEB_DEVELOPMENT": 0}}),
#     ("We need to develop a new feature for user authentication.", {"cats": {"STORAGE": 0, "AUTHENTICATION": 1, "DATA_ANALYSIS": 0, "COMMUNICATION": 0, "WEB_DEVELOPMENT": 0}}),
#     ("Our project needs to analyze large datasets.", {"cats": {"STORAGE": 0, "AUTHENTICATION": 0, "DATA_ANALYSIS": 1, "COMMUNICATION": 0, "WEB_DEVELOPMENT": 0}}),
#     ("We are planning to implement a chat feature.", {"cats": {"STORAGE": 0, "AUTHENTICATION": 0, "DATA_ANALYSIS": 0, "COMMUNICATION": 1, "WEB_DEVELOPMENT": 0}}),
#     ("I need to create a responsive web application.", {"cats": {"STORAGE": 0, "AUTHENTICATION": 0, "DATA_ANALYSIS": 0, "COMMUNICATION": 0, "WEB_DEVELOPMENT": 1}})
# ]


with open('AI_LLM_Analysis/training_data.json', 'r') as file:
    TRAINING_DATA_FILE = json.load(file)

TRAINING_DATA = [(item["text"], {"cats": item["cats"]}) for item in TRAINING_DATA_FILE["training_data"]]
# Initialize the optimizer after the pipeline is set up
optimizer = nlp.initialize()

# Training loop
for epoch in range(10):
    random.shuffle(TRAINING_DATA)
    losses = {}
    for text, annotations in TRAINING_DATA:
        doc = nlp.make_doc(text)
        example = Example.from_dict(doc, annotations)
        nlp.update([example], sgd=optimizer, losses=losses)
    print(f"Epoch {epoch} - Losses: {losses}")


# Mapping intents to technologies
intent_to_tech_map = {
    "STORAGE": ["SQL", "NoSQL", "Azure Storage", "Amazon S3"],
    "AUTHENTICATION": ["OAuth", "JWT", "LDAP", "Active Directory"],
    "DATA_ANALYSIS": ["Python", "Pandas", "Spark", "Hadoop"],
    "COMMUNICATION": ["WebSockets", "REST APIs", "GraphQL", "Firebase"],
    "WEB_DEVELOPMENT": ["HTML", "CSS", "JavaScript", "React", "Angular", "C#"]
}

# Function to extract technologies based on intent
def extract_technologies_based_on_intent(text):
    doc = nlp(text)
    intent = None
    for label, score in doc.cats.items():
        if score > 0.5:  # Set a threshold for the confidence score
            intent = label
            break
    
    if intent:
        technologies = intent_to_tech_map.get(intent, [])
        return intent, technologies
    else:
        return "Unknown", []

# Example usage
text = "I want to build an app that stores customer recipes to be accessed by everyone."
intent, technologies = extract_technologies_based_on_intent(text)
print(f"Intent: {intent}")
print(f"Suggested Technologies: {technologies}")







