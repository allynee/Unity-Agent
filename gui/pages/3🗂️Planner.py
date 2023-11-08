import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def get_three_plans(task):
	planner= A.Planner()
	memory_manager = A.MemoryManager()
	examples = memory_manager._get_plan(task)
	st.write("Previous plans:\n", examples)
	output_1 = planner._generate_plan(task, examples)
	output_2 = planner._generate_plan(task, examples)
	output_3 = planner._generate_plan(task, [])
	st.write("Output 1 is:\n\n ", output_1)
	st.write("Output 2 is: \n\n", output_2)
	st.write("Output 3 is: \n\n", output_3)

def get_plan(task):
	planner = A.Planner()
	memory_manager = A.MemoryManager()
	examples = memory_manager._get_plan(task)
	st.write("Previous plans:\n", examples)
	output = planner._generate_plan(task, examples)
	st.write("Plan generated is:\n\n ", output)

st.title("Testing planning agent")

st.write("Generate plan")
task= st.text_area(f"Enter task here", key="task1")
if st.button("Run", key="run1"):
	with st.spinner("Processing"):
		get_plan(task)
		st.success("Process done!")

st.write("Generate 3 plans")
task = st.text_area(f"Enter task here", key="task3")
if st.button("Run", key="run3"):
	with st.spinner("Processing"):
		get_three_plans(task)
		st.success("Process done!")

