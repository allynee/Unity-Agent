from dotenv import load_dotenv, find_dotenv
import guidance
import openai
import os

class Planner:
    def __init__(self, model_name="gpt-3.5-turbo", temperature=0, resume=False, ckpt_dir="ckpt"):
        load_dotenv(find_dotenv())
        openai.api_key = os.getenv("OPENAI_API_KEY")
        guidance.llm = guidance.llms.OpenAI(model_name, temperature=temperature)
        self.llm=guidance.llm

    #TODO: Create plan function
    def _generate_plan(self, user_query):
        return ""
    
    def _orchestrate(self, user_query):
        task_type = self._identify_task_type(user_query)
        prefabs = self._identify_prefab(user_query)
        instructions = f"1.{task_type} {prefabs}\n"

        if task_type == "Create":
            location, orientation, size, color = self._identify_create_object_operations(user_query)
        else:
            location, orientation, size, color = self._identify_modify_object_operations(user_query)

        if location=="True":
            instructions+=f"2. Set location of object to be {self._identify_location(user_query)})\n"
        else:
            instructions+=f"2. Set location of object to be in front of the user\n"
        
        return instructions
    
    def _identify_task_type(self, user_query):
        prompt = guidance('''
        {{#system~}}
        You are a helpful and terse Unity assistant. 
        You are helping a user design a room in Unity.
        {{~/system}}
        {{#user~}}
        Given a user instruction, identify if the user wants to create a new object or modify an object already in the room.
        For example, "Give me a chair" would be a creation task,
        while "move the chair here" and "make the chair smaller" are modification tasks since they refer to already created object(s) in the room.
        The user query is {{user_query}}.
        You must only return a single word: "Create" or "Modify".
        {{~/user}}
        {{#assistant~}}
        {{gen "task_type" max_tokens=5 temperature=0}}
        {{~/assistant}}
        ''')
        return prompt(user_query=user_query)["task_type"]
    
    def _identify_prefab(self, user_query):
        #Given the user query, identify the prefabs to be instantiated
        prompt = guidance('''
        {{#system~}}
        You are a helpful and terse Unity assistant. Given a user instruction, identify which prefab(s) need to be created or modified in the room.
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

    def _identify_create_object_operations(self, user_query):
        prompt = guidance('''
        {{#system~}}
        You are a helpful and terse Unity assistant.
        You are helping a user create an object in a room in Unity.
        {{~/system}}
        {{#user~}}
        Given a user instruction, identify whether there is a specification about where the object should be placed or located. 
        Look for words or phrases that indicate location or placement like 'next to', 'on the left of', 'behind', 'in front of', etc. 
        The user instruction is: {{user_query}}.
        Return 'True' if there is a location specification, and 'False' if not. 
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "location" max_tokens=5 temperature=0}}
        {{~/assistant}}
        {{#user~}}
        Given a user instruction {{user_query}}, identify if the user specified an orientation the object should be placed. 
        Examples include "upright", "sideways", "horizontal", etc.
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "orientation" max_tokens=5 temperature=0}}
        {{~/assistant}}
        {{#user~}}
        Given a user instruction, identify if any descriptive word related to size is used to describe the object.
        The user instruction is: {{user_query}}.
        Return "True" if a size descriptor is provided, and "False" if not. 
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "size" max_tokens=5 temperature=0}}
        {{~/assistant}}
        {{#user~}}
        Given a user instruction {{user_query}}, identify if the user specified what color the object should be.
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "color" max_tokens=5 temperature=0}}
        {{~/assistant}}
        ''')
        resp = prompt(user_query=user_query)
        return resp["location"], resp["orientation"], resp["size"], resp["color"]
    
    def _identify_modify_object_operations(self, user_query):
        prompt=guidance('''
        {{#system~}}
        You are a helpful and terse Unity assistant. 
        You are helping a user modify an object in a room in Unity.
        {{~/system}}
        {{#user~}}
        Given a user instruction {{user_query}}, identify if the user wants to change the object's location.
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "change_location" max_tokens=5 temperature=0}}
        {{~/assistant}}
        {{#user~}}
        Given a user instruction {{user_query}}, identify if the user wants to change the object's orientation.
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "change_orientation" max_tokens=5 temperature=0}}
        {{~/assistant}}
        {{#user~}}
        Given a user instruction {{user_query}}, identify if the user wants to change the object's size.
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "change_size" max_tokens=5 temperature=0}}
        {{~/assistant}}
        {{#user~}}
        Given a user instruction {{user_query}}, identify if the user wants to change the object's color.
        You must only return a single word: "True" or "False".
        {{~/user}}
        {{#assistant~}}
        {{gen "change_color" max_tokens=5 temperature=0}}
        {{~/assistant}}
        ''')
        resp = prompt(user_query=user_query)
        return resp["change_location"], resp["change_orientation"], resp["change_size"], resp["change_color"]

    def _identify_location(self, user_query):
        #Given the user query, identify where the prefab is to be placed
        #TODO: Understand this portion more.
        prompt = guidance('''
        {{#system~}}
        You are a helpful and terse Unity assistant. 
        {{~/system}}
        {{#user~}}               
        Given a user instruction, identify the desired position or location specification.
        Here is a list of possible locations: ["BackWall", "FrontWall", "LeftWall", "RightWall", "Floor", "Ceiling"]
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
    

    ## Unsure if this piece of code would help
    def _identify_all_tasks(self, user_query):
        prompt = guidance('''
        {{#system~}}
        You are a helpful terse Unity assistant. 
        {{~/system}}
        {{#user~}}
        Given a user instruction, identify the the task is a Single element reference, multiple element reference, or room structure reference.
        Single element reference refers to instructions revolved around a singular object or feature (e.g., move the chair here)
        Multiple element reference refers to multiple elements reference: Instructions involving more than 1 object/feature but don't necessarily require knowledge of entire environment's layout (e.g., align the table with the sofa)
        Room structure reference: Interactions demanding understanding of space's layout or structure. Either reference most of the room's components, or require awareness of the room's structural elements (e.g., put a lamp on all 4 walls) 
        The user query is {{user_query}}.
        You must only return "single element reference", "multiple element reference", or "room structure reference".
        {{~/user}}
        {{#assistant~}}
        {{gen "number_of_elements" max_tokens=100 temperature=0}}
        {{~/assistant}}
        ''')
        resp = prompt(user_query=user_query)
        return resp["number_of_elements"]
        # You must only return a number, like 1, 2, 3, etc.
