import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def get_function(task, examples):
	coder = A.CodeAgent()
	output = coder._generate_function(task=task, examples=examples)
	st.write(output)

def get_script(task, plan, functions):
	coder = A.CodeAgent()
	output = coder._generate_script(task=task, plan=plan, functions=functions)
	st.write(output)
	print(output)

st.title("Testing code agent 🤖")

st.write("1. Generate function")
instruction = st.text_area(f"Enter instruction here", key="instruction")
examples = st.text_area(f"Enter examples here", key="examples")
if st.button("Run", key="generate_function"):
	with st.spinner("Processing"):
		get_function(instruction, examples)
		st.success("Process done!")

st.write("2. Generate script")
task = st.text_area(f"Enter task here", key="task")
plan = st.text_area(f"Enter plan here", key="plan")
functions = st.text_area(f"Enter functions here", key="functions")
if st.button("Run", key="generate_script"):
	with st.spinner("Processing"):
		get_script(task, plan, functions)
		st.success("Process done!")
