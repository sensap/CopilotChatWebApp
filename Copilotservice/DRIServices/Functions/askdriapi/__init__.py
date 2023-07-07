import logging
import azure.functions as func

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
    logging.info('User asked Question: %s', question)
    if question:
      
    else:
        return func.HttpResponse("Please pass a question on the query string or in the request body", status_code=400)
    
    return func.HttpResponse(
            "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
            status_code=200
    )

def get_dri_skill(question):
#     template = """\
# "You are an intelligent assistant helping Contoso Inc employees with their healthcare plan questions and employee handbook questions. " + \
# "Use 'you' to refer to the individual asking the questions even if they ask with 'I'. " + \
# "Answer the following question using only the data provided in the sources below. " + \
# "For tabular information return it as an html table. Do not return markdown format. "  + \
# "Each source has a name followed by colon and the actual information, always include the source name for each fact you use in the response. " + \
# "If you cannot answer using the sources below, say you don't know. " + \
# """

# ###
# Question: 'What is the deductible for the employee plan for a visit to Overlake in Bellevue?'

# Sources:
# info1.txt: deductibles depend on whether you are in-network or out-of-network. In-network deductibles are $500 for employee and $1000 for family. Out-of-network deductibles are $1000 for employee and $2000 for family.
# info2.pdf: Overlake is in-network for the employee plan.
# info3.pdf: Overlake is the name of the area that includes a park and ride near Bellevue.
# info4.pdf: In-network institutions include Overlake, Swedish and others in the region

# Answer:
# In-network deductibles are $500 for employee and $1000 for family [info1.txt] and Overlake is in-network for the employee plan [info2.pdf][info4.pdf].

# ###
# Question: '{q}'?

# Sources:
# {retrieved}

# Answer:
# """