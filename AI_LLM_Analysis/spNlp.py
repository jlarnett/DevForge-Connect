import spacy
from spacy.training import Example
import random

# Load a pre-trained model with a tagger
nlp = spacy.load("en_core_web_md")

# Check POS tagging before training
doc = nlp("Apple is looking at buying a startup.")
for token in doc:
    print(token.text, token.pos_)  # POS tagging works here

# Add text categorizer to the pipeline
textcat = nlp.add_pipe("textcat", last=True)
textcat.add_label("TECH")

# Example training data for textcat
TRAINING_DATA = [
    ("We are building a cloud-based platform.", {"cats": {"TECH": 1}}),
    ("Our team needs a web developer.", {"cats": {"TECH": 1}}),
]

# Initialize the optimizer
optimizer = nlp.resume_training()

# Train the textcat without affecting the tagger
for epoch in range(5):
    random.shuffle(TRAINING_DATA)
    for text, annotations in TRAINING_DATA:
        doc = nlp.make_doc(text)
        example = Example.from_dict(doc, annotations)
        nlp.update([example], sgd=optimizer)

# Check if POS tagging still works after training
doc = nlp("Apple is looking at buying a startup.")
for token in doc:
    print(token.text, token.pos_)  # Check if POS tagging still works





