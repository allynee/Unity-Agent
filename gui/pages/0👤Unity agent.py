import base64
import streamlit as st
import os
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def edit_code_string(code_string):
    try:
        first_index = code_string.index("using")
        last_index = code_string.rindex("}")  
        return code_string[first_index:last_index+1]  
    except ValueError:
        st.write("Invalid code: 'using' or '}' not found.")

def create_and_download_cs_file(code_string):
    code_string = edit_code_string(code_string)
    file_name = "generated_script.cs"
    with open(file_name, "w", encoding="utf-8") as file:
        file.write(code_string)
    
    with open(file_name, "rb") as file:
        btn = st.download_button(
            label="Download .cs file",
            data=file,
            file_name=file_name,
            mime='text/plain',
        )

    if btn:
        os.remove(file_name)

def initialize_components():
    planner = A.Planner()
    coder = A.Coder()
    memorymanager = A.MemoryManager()
    # TODO: Critic
    return planner, coder, memorymanager

def generate_initial_script(task):
    st.write(f"- Received your task to generate a script for: {task}")
    st.write("- Intializing components...")
    planner, coder, memorymanager = initialize_components()
    st.write("- Retrieving similar plans...")
    plan_examples = memorymanager._get_plan(task)
    st.write("- Generating plan...")
    plan = planner._generate_plan(task, plan_examples)
    st.write("Here is the generated plan!")
    plans = plan.split("\n")
    st.write(plans)
    functions = []
    st.write("Generating functions...")
    for plan in plans:
        function_examples = memorymanager._get_code(plan)
        functions.append(coder._generate_function(plan, function_examples))
    st.write("Here are the generated functions!")
    st.write(functions)
    st.write("Generating script...")
    script = coder._generate_script(task, plan, functions)
    st.write("Here is the generated script!")
    st.write(script)
    st.write("\n\nDownload the script here:")
    create_and_download_cs_file(script)
    # TODO: Finish

def simulate_self_repair():
    # Max 3 rounds
    pass

def refine_code_pipeline():
    pass

def refine_strategy_pipeline():
    pass

def add_new_experience():
    pass

# TODO: May need to store a class of generated outputs to ensure learning can take place

st.title("Testing entire pipeline")

st.write("1. Generate initial script")
task = st.text_area(f"Enter task here", key="task")
if st.button("Run", key="generate_script"):
    with st.spinner("Processing"):
        generate_initial_script(task)
        st.success("Process done!")