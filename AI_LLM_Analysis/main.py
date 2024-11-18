from typing import List, Union

from pydantic import BaseModel
from textGen import generate_response
from fastapi import FastAPI
from predict_from_model import spacy_prediction
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

class TextPredictionRequest(BaseModel):
    text: List[str]

origins = [
    "http://localhost:7009",
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/")
def read_root():
    return {"Hello": "World"}

@app.get("/items/{item_id}")
def read_item(item_id: int, q: Union[str, None] = None):
    return {"item_id": item_id, "q": q}

@app.get("/genResponse")
def genResponse(text):
    response = generate_response(text)
    return response

# @app.get("/textPrediction")
# def textPrediction(text):
#     response = spacy_prediction(text)
#     return response

@app.post("/textPrediction")
def textPrediction(text: TextPredictionRequest):
    responseList = []
    for item in text.text:
        response = spacy_prediction(item)
        responseList.append(response)
    return responseList