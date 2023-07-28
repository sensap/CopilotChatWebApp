import logging
import os
import azure.functions as func
from azure.identity import DefaultAzureCredential
from azure.search.documents import SearchClient
from azure.storage.blob import BlobServiceClient
from azure.search.documents.models import QueryType
 
import json


def main(req: func.HttpRequest) -> func.HttpResponse:
  
    if req.method == "GET":
        searchstring = req.params.get('searchstring')
        if searchstring is None:
            # searchstring = "What is included in my Northwind Health Plus plan that is not in standard?"
            return func.HttpResponse(
                "Please pass a searchstring on the query string",
                status_code=400
            )
        topn = req.params.get('topn')
        if topn is None:
            topn = 3
        
        semantic_captions = req.params.get('semantic_captions')
        exclude_category = req.params.get('exclude_category')
        semantic_ranker = req.params.get('semantic_ranker')
        skill_name = req.params.get('skill_name')
        if semantic_ranker is None:
            semantic_ranker = True
        
    if req.method == "POST":
        req_body = req.get_json()
        if req_body:
            searchstring = req_body.get('searchstring')
            topn = req_body.get('topn')
            semantic_captions = req_body.get('semantic_captions')
            exclude_category = req_body.get('exclude_category')
            semantic_ranker = req_body.get('semantic_ranker')
            skill_name = req_body.get('skill_name')
            if semantic_ranker is None:
                semantic_ranker = True
    logging.info('User asked Question: %s', searchstring)
    try:
        azure_credential = DefaultAzureCredential()
    except Exception as e:
        logging.info(e)
        return func.HttpResponse(
            "Credential Error",
            status_code=500
        )
    # Read environment variables
    try:
        AZURE_SEARCH_SERVICE = os.environ["AZURE_SEARCH_SERVICE"]
        AZURE_SEARCH_INDEX = os.environ["AZURE_SEARCH_INDEX"]
    except KeyError:
        logging.info(KeyError.with_traceback())
        return func.HttpResponse(
            "Key Error",
            status_code=500 )
    try:
        # initialize search client
        searchclient = SearchClient( endpoint=AZURE_SEARCH_SERVICE,
                                    index_name=AZURE_SEARCH_INDEX, 
                                    credential=azure_credential)
        
        # use semantic search if semantic_captions is true
        use_semantic_captions = True if semantic_captions == "true" else False
        # use filter if exclude_category is specified
        filter = "category ne '{}'".format(exclude_category) if exclude_category else None 
        # use semantic ranker if semantic_ranker is true
        if semantic_ranker:
            r =  searchclient.search(searchstring,
                                    filter=filter,
                                    query_type=QueryType.SEMANTIC, 
                                    query_language="en-us", 
                                    query_speller="lexicon", 
                                    semantic_configuration_name="default", 
                                    top=topn, 
                                    query_caption="extractive|highlight-false" if use_semantic_captions else None)
        else:
            r = searchclient.search(search_text=searchstring, top=topn, filter=filter)
        if use_semantic_captions:
            # serialize the results to json
            results = json.dump([{"sourcepage": doc["sourcepage"], "content": nonewlines(doc["content"]), "caption": doc["captions"][0]["text"]} for doc in r])
        else:
            # serialize the results to json
            results =  json.dumps([{"sourcepage": doc["sourcepage"], "content": nonewlines(doc["content"])} for doc in r])
        content = results 

        if content is not None:      
            return func.HttpResponse(
                    content,
                    status_code=200
            )
        else:
            return func.HttpResponse(
                [],
                status_code=200
            )
    except Exception as e:
        logging.info(e)
        return func.HttpResponse(
            "Error",
            status_code=500
        )
def nonewlines(s: str) -> str:
    return s.replace('\n', ' ').replace('\r', ' ')

def retrieve_skill(skill_name: str, azure_credential: DefaultAzureCredential) -> str:
    try:
        AZURE_STORAGE_CONNECTION_STRING = os.environ["AZURE_STORAGE_CONNECTION_STRING"]
    except KeyError:
        logging.info(KeyError.with_traceback())
        return func.HttpResponse(
            "Key Error",
            status_code=500 )
    blob_service_client = BlobServiceClient.from_connection_string(AZURE_STORAGE_CONNECTION_STRING)
    container_client = blob_service_client.get_container_client("skills")
    container_client.get_blob_client(skill_name).download_blob().readall()
     