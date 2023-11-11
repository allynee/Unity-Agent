import base64
import streamlit as st
import os
import re
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A
from pydantic import BaseModel, Field, validator, ConfigDict
from typing import List, Optional

class OutputCls:
    def __init__(self, *, task:str, feedback: Optional[str] = None, plan: list[str], functions: list[str], script: str):
        self.task = task
        self.feedback = feedback
        self.plan = plan
        self.functions = functions
        self.script = script

class Output(BaseModel):
    model_config = ConfigDict(from_attributes=True)
    task: str
    feedback: Optional[str] = None
    plan: list[str]
    functions: list[str]
    script: str

def get_class_name_from_code(code_string):
    # Extract the class name from the code string using regex
    match = re.search(r'public class (\w+)', code_string)
    if match:
        return match.group(1)
    return "generated_script" 

def create_and_download_cs_file(code_string):
    class_name = get_class_name_from_code(code_string)
    file_name = f"{class_name}.cs"
    file_path = f"agent_generated_scripts/{file_name}"
    with open(file_path, "w", encoding="utf-8") as file:
        file.write(code_string)
    with open(file_path, "rb") as file:
        btn = st.download_button(
            label="Download .cs file",
            data=file,
            file_name=file_name,
            mime='text/plain',
        )
    if btn:
        os.remove(file_name)

def generate_initial_script(task):
    st.write(f"- Received your task to generate a script for: {task}")
    st.write("- Retrieving similar plans...")
    plan_examples = ss.memorymanager._get_plan(task)
    st.write("- Generating plan...")
    plan = ss.planner._generate_plan(task, plan_examples)
    st.write("Here is the generated plan!")
    plans = plan.split("\n")
    st.write(plans)
    ss.generated_output = OutputCls(
        task=task,
        plan=plans,
        functions=None,
        script=None
    )
    # functions = []
    # st.write("Generating functions...")
    # for plan in plans:
    #     function_examples = ss.memorymanager._get_code(plan)
    #     st.write("Function examples:\n\n")
    #     st.write(function_examples)
    #     functions.append(ss.coder._generate_function(plan, function_examples))
    #     st.write("Function:")
    #     st.write(functions[-1])
    # st.write("Here are the generated functions!")
    # st.write(functions)
    # st.write("Generating script...")
    # script = ss.coder._generate_script(task, plan, functions)
    # st.write("Here is the generated script!")
    # st.write(script)
    # st.write("\n\nDownload the script here:")
    # create_and_download_cs_file(script)
    # ss.generated_output = OutputCls(
    #     task=task,
    #     plan=plans,
    #     functions=functions,
    #     script=script
    # )
    # st.write(ss.generated_output)

def refine_plan_pipeline(feedback):
    ss.generated_output.feedback = feedback
    st.write("Original Plan:")
    st.write(ss.generated_output.plan)
    new_plan = ss.critic._refine_plan(ss.generated_output)
    st.write("New Plan:")
    st.write(new_plan)
    new_plans = new_plan.split("\n")
    
    # index_list, new_steps = ss.critic._refine_plan(ss.generated_output)
    # st.write(index_list)
    # st.write(new_steps)
    # ss.generated_output = ss.critic._refine_plan(ss.generated_output)

def refine_code_pipeline(feedback):
    ss.generated_output.feedback = feedback
    ss.generated_output = ss.critic._refine_code(ss.generated_output)
    #TODO: Undone

def add_new_experience():
    ss.memorymanager._add_new_experience(ss.generated_output)

st.title("Testing entire pipeline")
ss = st.session_state
if "generated_output" not in st.session_state:
    ss.planner = A.Planner()
    ss.coder = A.Coder()
    ss.memorymanager = A.MemoryManager()
    ss.critic = A.Critic()
    ss.generated_output = None

st.write("1. Generate initial script")
task = st.text_area(f"Enter task here", key="task")
if st.button("Run", key="generate_script"):
    with st.spinner("Processing"):
        generate_initial_script(task)
        st.success("Process done!")

st.write("2. Refine plan")
feedback = st.text_area(f"Enter feedback here", key="feedback")
if st.button("Run", key="refine_plan"):
    with st.spinner("Processing"):
        refine_plan_pipeline(feedback)
        st.success("Process done!")
