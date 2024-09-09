from typing import Union
from AI_LLM_Analysis.textGen import generate_response
from fastapi import FastAPI

app = FastAPI()


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