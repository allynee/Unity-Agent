import guidance
from dotenv import load_dotenv, find_dotenv
import os
import openai

class CodeAgent:
    def __init__(self, model_name="gpt-3.5-turbo", temperature=0, resume=False, ckpt_dir="ckpt", execution_error=True):
        load_dotenv(find_dotenv())
        openai.api_key = os.getenv("OPENAI_API_KEY")
        print("===============================")
        print(os.getenv("OPENAI_API_KEY"))
        print("===============================")
        guidance.llm = guidance.llms.OpenAI(model_name)
        self.llm=guidance.llm
        self.ckpt_dir = ckpt_dir
        self.execution_error = execution_error
        if resume:
            pass
        else:
            pass

    def _generate_code(self, task): 
        print(self.llm)
        coder = guidance('''
        {{#system~}}
        You are a unity programmer helping to translate pseudocode for a task in unity to C# code. 
        {{~/system}}
        {{#user~}}
        {{task}}
        {{~/user}}
        {{#assistant~}}
        {{gen "code" temperature=0}}
        {{~/assistant}}
        ''')
        resp = coder(task=task)
        return resp["code"]