import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def get_function(task):
	coder = A.Coder()
	memorymanager = A.MemoryManager()
	function_examples = memorymanager._get_code(task)
	st.write("Function examples:\n\n")
	st.write(function_examples)
	output = coder._generate_function(task=task, examples=function_examples)
	st.write(output)

def get_script(task, plan, functions):
	coder = A.Coder()
	output = coder._generate_script(task=task, plan=plan, functions=functions)
	st.write(output)

st.title("Testing code agent 🤖")

st.write("1. Generate function")
instruction = st.text_area(f"Enter instruction here", key="instruction")
if st.button("Run", key="generate_function"):
	with st.spinner("Processing"):
		get_function(instruction)
		st.success("Process done!")

st.write("2. Generate script")
task = st.text_area(f"Enter task here", key="task")
plan = st.text_area(f"Enter plan here", key="plan")
functions = st.text_area(f"Enter functions here", key="functions")
if st.button("Run", key="generate_script"):
	with st.spinner("Processing"):
		get_script(task, plan, functions)
		st.success("Process done!")
