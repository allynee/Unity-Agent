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
    def __init__(self, *, task:str, feedback: Optional[str] = None, plan: list[str], functions: list[str], script: str, old_plan_function_map: dict, new_plan_function_map: dict):
        self.task = task
        self.feedback = feedback
        self.plan = plan
        self.functions = functions
        self.script = script
        self.old_plan_function_map = old_plan_function_map 
        self.new_plan_function_map = new_plan_function_map 

class Output(BaseModel):
    model_config = ConfigDict(from_attributes=True)
    task: str
    feedback: Optional[str] = None
    plan: list[str]
    functions: list[str]
    script: str
    old_plan_function_map: dict 
    new_plan_function_map: dict 

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

def remove_numbered_bullets(text):
    pattern = r'\d+\.\s'
    cleaned_text = re.sub(pattern, "", text)
    return cleaned_text.strip()

def clean_function_text(text):
    text = text.replace("```csharp\n", "")  # Replace the starting string with nothing
    text = text.replace("```", "")          # Replace the ending string with nothing
    return text.strip()

def generate_initial_script(task):
    st.markdown(f"## Received your task to generate a script for: {task}")
    st.markdown("## Generating the plan...")
    plan_examples = ss.memorymanager._get_plan(task)
    plan = ss.planner._generate_plan(task, plan_examples)
    st.write(plan)
    plans = plan.split("\n")
    plan_function_map = {}
    functions = []
    st.markdown("## Generating functions...")
    for plan in plans:
        st.write("Generating function for \n ```" + plan + "```")
        function_examples = ss.memorymanager._get_code(plan)
        function = ss.coder._generate_function(plan, function_examples)
        function = clean_function_text(function)
        functions.append(function)
        plan_function_map[remove_numbered_bullets(plan)] = function
        st.write("\n```csharp\n" + function + "\n\n")
    st.markdown("## Generating the entire script...")
    script = ss.coder._generate_script(task, plan, functions)
    st.write("```csharp\n" + script)
    st.write("\n\nDownload the script here:")
    create_and_download_cs_file(script)
    ss.generated_output = OutputCls(
        task=task,
        plan=plans,
        functions=functions,
        script=script,
        old_plan_function_map=plan_function_map,
        new_plan_function_map= plan_function_map
    )
    st.write("\n\nThe stored object:")
    st.write(ss.generated_output)

def refine_plan_pipeline(feedback):
    ss.generated_output.feedback = feedback
    st.markdown("## Original Plan:")
    st.write("\n\n".join(ss.generated_output.plan))
    new_plan = ss.critic._refine_plan(ss.generated_output)
    st.markdown("## New Plan:")
    st.write(new_plan)
    new_plans = new_plan.split("\n")
    new_plans = [plan for plan in new_plans if plan.strip()]
    plan_function_map = {}
    st.markdown("## Generating New Functions...")
    for plan in new_plans:
        cleaned_instruction = remove_numbered_bullets(plan)
        if cleaned_instruction in ss.generated_output.old_plan_function_map:
            plan_function_map[cleaned_instruction] = ss.generated_output.old_plan_function_map[cleaned_instruction]
        else:
            st.markdown("Generating a new function for \n ```" + plan + "```")
            function_examples = ss.memorymanager._get_code(cleaned_instruction)
            function = ss.coder._generate_function(cleaned_instruction, function_examples)
            function = clean_function_text(function)
            plan_function_map[cleaned_instruction] = function
            st.markdown("New function: \n```csharp\n" + function)
    new_functions = list(plan_function_map.values())
    script = ss.coder._generate_script(task, new_plan, new_functions)
    st.markdown("## Here is the newly generated script!")
    st.markdown("```csharp\n" + script)
    st.markdown("\n\n## Download the script here:")
    create_and_download_cs_file(script)
    ss.generated_output.new_plan_function_map = plan_function_map
    st.markdown("\n\n## The stored object:")
    st.write(ss.generated_output)

def refine_code_pipeline(feedback):
    ss.generated_output.feedback = feedback
    ss.generated_output = ss.critic._refine_code(ss.generated_output)
    #TODO: Undone

def add_new_experience():
    if ss.generated_output.new_plan_function_map is None:
        st.write("You have not generated any scripts yet!")
        return
    else:
        plan, code = ss.memorymanager._add_new_experience(ss.generated_output)
        st.write("Added new experience to memory!")
        st.write("Plan added:")
        st.write(plan)
        st.write("Code added:")
        st.write(code)

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
if st.button("Generate", key="generate_script"):
    with st.spinner("Processing"):
        generate_initial_script(task)
        st.success("Process done!")

st.write("2. Refine plan")
feedback = st.text_area(f"Enter feedback here", key="feedback")
if st.button("Refine", key="refine_plan"):
    with st.spinner("Processing"):
        refine_plan_pipeline(feedback)
        st.success("Process done!")

st.write("3. Refine code")
logs = st.text_area(f"Enter logs here", key="logs")
if st.button("Refine", key="refine_code"):
    with st.spinner("Processing"):
        refine_code_pipeline(logs)
        st.success("Process done!")

st.write("4. Add new experience")
if st.button("Add experience", key="add_new_experience"):
    with st.spinner("Processing"):
        add_new_experience()
        st.success("Process done!")