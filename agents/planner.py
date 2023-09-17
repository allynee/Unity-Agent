from dotenv import load_dotenv, find_dotenv
import guidance
import openai
import os

class PlannerAgent:
    def __init__(self, model_name="gpt-3.5-turbo", temperature=0, resume=False, ckpt_dir="ckpt"):
        load_dotenv(find_dotenv())
        openai.api_key = os.getenv("OPENAI_API_KEY")
        print("===============================")
        print(os.getenv("OPENAI_API_KEY"))
        print("===============================")
        guidance.llm = guidance.llms.OpenAI(model_name, temperature=temperature)
        self.llm=guidance.llm
    
    def _identify_prefab(self, user_query):
        #Given the user query, identify the prefabs to be instantiated
        prompt = guidance('''
        {{#system~}}
        You are a helpful Unity assistant. Given a user instruction, identify which prefab(s) need to be created, modified, or deleted in the room.
        {{~/system}}
        {{#user~}}               
        The list of prefabs are: ["Chair", "TableLamp", "Table", "Screen", "Push Button"]
        The user query is {{user_query}}.
        You must only return a list of strings, like ["Chair","Table"] for example.
        If there are no objects to be created, modified, or deleted, return an empty list like [].
        {{~/user}}
        {{#assistant~}}
        {{gen "prefab" temperature=0}}
        {{~/assistant}}
        ''')
        resp = prompt(user_query=user_query)
        return resp["prefab"]

    def _identify_location(self, user_query):
        #Given the user query, identify where the prefab is to be placed
        #TODO: Understand this portion more.
        prompt = guidance('''
        {{#system~}}
        You are a helpful Unity assistant. 
        {{~/system}}
        {{#user~}}               
        Given a user instruction, identify the desired position or location specification.
        Here is a list of possible locations: ["BackWall", "FrontWall", "LeftWall", "RightWall", "Floor", "Ceiling", "UserPosition"]
        The user query is {{user_query}}
        You must only return a list of strings, like ["LeftWall","RightWall"] for example.
        If no specific position is mentioned, return an empty list like [].        
        {{~/user}}
        {{#assistant~}}
        {{gen "location" temperature=0}}
        {{~/assistant}}
        ''')
        resp = prompt(user_query=user_query)
        return resp["location"]

    def _orchestrate(self):
        pass