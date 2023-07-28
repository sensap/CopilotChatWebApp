import logging
import azure.functions as func
import requests
import os
from azure.storage.blob import BlobServiceClient
from azure.identity import DefaultAzureCredential
import semantic_kernel as sk
from semantic_kernel.connectors.ai.open_ai import AzureTextCompletion, OpenAITextCompletion

kernel = sk.SemanticKernel()

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    question = req.params.get('question')
    if not question:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            name = req_body.get('question')
        host = req.headers.get('Host')
        logging.info('Host: %s', host)

    if question:
        credential = DefaultAzureCredential()
        url = req.url.replace('askdriapi', 'dricognitivesearchapi')
        url = url.replace('question', 'searchstring')
        #url = 'http://' + host + '/api/dricognitivesearch?question=' + question
        logging.info('User asked Question: %s', question)
        response = requests.api.get(url)

        return func.HttpResponse(
        response.text,
            status_code=200
    )
    else:
        return func.HttpResponse("Please pass a question on the query string or in the request body", status_code=400)
    
   

def retrieve_skill(skill_name: str, azure_credential: DefaultAzureCredential) -> str:
     