from dotenv import load_dotenv, find_dotenv
import guidance
import openai
import os

class Planner:
    # gpt-3.5-turbo-1106
    # gpt-4-0613
    def __init__(self, model_name="gpt-3.5-turbo-1106", temperature=0.0, resume=False, ckpt_dir="ckpt"):
        load_dotenv(find_dotenv())
        openai.api_key = os.getenv("OPENAI_API_KEY")
        guidance.llm = guidance.llms.OpenAI(model_name, temperature=temperature)
        self.llm=guidance.llm

    def _generate_plan(self, task, examples):
        planner = guidance('''
        {{#system~}}
        You are an efficient, direct and helpful assistant tasked with helping shape a ubicomp space. 
        {{~/system}}
        {{#user~}}
        As an assistant, create clear and precise instructions to alter a 3D ubicomp space according to user requests. Follow these guidelines:
        - Respond with a numbered set of instructions. Each instruction should be a self-contained action or directive that could be executed independently but contributes to the overall task when combined with others.
        - Each instruction should modify only 1 property or behaviour.
        - If you need to edit the position of more than one object, include it within a single instruction. For example, use "Edit the Position property of each chair to be 0.5 meters in front of each room wall" instead of separate instructions for each chair.
        - Properties that can be edited are: Position, Rotation, Size, Color, Illumination (Whether the object emanates light), Luminous Intensity (Brightness of the light between 1 and 10), Levitation (When an object is not levitated, it follows the rules of gravity, and when levitated, it floats). 
        - Your instructions must translate subjective terms into specific, measurable instructions. For example, terms like "big" or "close to me" can translate to “2 times its current size” and  “0.2m away from the user” respectively. Always cite explicit numbers.
        - For colors, use RGBA values.
        - When citing these quantitative measures, prioritize relative measurements derived from the user, objects, or scene. Avoid arbitrary values.
        - The first instruction should be to either create a new object, or to find an existing object in the user's field of view.
        
        The space consists of 4 walls, 1 ceiling, and 1 floor.
        
        You are limited to creating or modifying the following object types: (You must use the exact case-sensitive type of the object)
        Chair, Fox, Lamp, LED Cube, Push Button, Table, Vase, Zombie

        The user's prompt is {{task}}.
                                
        Here are examples of similar instructions which may or may not be applicable to you. Your response should mirror the structure and level of detail seen in the examples provided.
        \n {{examples}}

        The format for responses should strictly be:
            1. Instruction 1\n
            2. Instruction 2\n
            …

        *Note: Output should not contain any text other than the instructions.*
        {{~/user}}
        {{#assistant~}}
        {{gen "plan" max_tokens=1000 temperature=0}}
        {{~/assistant}}
        ''')
        resp = planner(task=task, examples=examples)
        return resp["plan"]
    