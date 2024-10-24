import spacy
from spacy.training import Example
import random
import json

# Create a blank 'en' model
nlp = spacy.blank("en")

# Add the text categorizer to the pipeline
# Ensure we only add it once
if "textcat_multilabel" not in nlp.pipe_names:
    textcat = nlp.add_pipe("textcat_multilabel", last=True)

# Add labels for the text categorizer
textcat.add_label("STORAGE")
textcat.add_label("AI")
textcat.add_label("DATA_ANALYSIS")
textcat.add_label("COMMUNICATION")
textcat.add_label("WEB_DEVELOPMENT")
textcat.add_label("GAME_DEVELOPMENT")
textcat.add_label("MOBILE_DEVELOPMENT")
textcat.add_label("IOT")
textcat.add_label("NETWORK")
textcat.add_label("OPERATING_SYSTEMS")


# Initialize the model after adding the textcat and labels
nlp.initialize()


with open('training_data.json', 'r') as file:
    TRAINING_DATA_FILE = json.load(file)

TRAINING_DATA = [(item["text"], {"cats": item["cats"]}) for item in TRAINING_DATA_FILE["training_data"]]
# Initialize the optimizer after the pipeline is set up
optimizer = nlp.initialize()

# Training loop
for epoch in range(20):
    random.shuffle(TRAINING_DATA)
    losses = {}
    for text, annotations in TRAINING_DATA:
        doc = nlp.make_doc(text)
        example = Example.from_dict(doc, annotations)
        nlp.update([example], sgd=optimizer, losses=losses)
    print(f"Epoch {epoch} - Losses: {losses}")

output_dir = "trained_models"

nlp.to_disk(output_dir)

# Mapping intents to technologies










