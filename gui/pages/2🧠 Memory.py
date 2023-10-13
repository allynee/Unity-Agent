import streamlit as st
from dotenv import load_dotenv
from time import time as now
import os
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def init_plan_memory(uploaded_file):
	save_folder = "../init_memory"
	file_name = uploaded_file.name
	os.makedirs(save_folder, exist_ok=True)
	file_path = os.path.join(save_folder, file_name)
	with open(file_path, "wb") as f:
		f.write(uploaded_file.getbuffer())
	memorymanager = A.MemoryAgent()
	output = memorymanager._init_plan_memory(file_path)
	st.write(output)

def init_code_memory(uploaded_file):
	save_folder = "../init_memory"
	file_name = uploaded_file.name
	os.makedirs(save_folder, exist_ok=True)
	file_path = os.path.join(save_folder, file_name)
	with open(file_path, "wb") as f:
		f.write(uploaded_file.getbuffer())
	memorymanager = A.MemoryAgent()
	output = memorymanager._init_code_memory(file_path)
	st.write(output)

def add_code(add):
	items = add.split(",")
	info = {
		"p_name":items[0],
		"p_code":items[1],
		"p_category":items[2],
		"p_compile":items[3],
		"p_ideal":items[4],
	}
	st.write(info)
	memorymanager = A.MemoryAgent()
	output = memorymanager._add_new_code(info)
	st.write(output)

def get_code(instruction):
	memorymanager = A.MemoryAgent()
	code_list = memorymanager._get_code(instruction)
	for i, c in enumerate(code_list):
		st.markdown(f"### Retrieved Code {i+1}")
		st.markdown(f"- Instruction: {c['instruction']}\n\n- Category: {c['category']}\n\n- Code:\n\n{c['code']}")
		st.write("====================")

def get_plan(user_query):
	memorymanager = A.MemoryAgent()
	plan_list = memorymanager._get_plan(user_query)
	for i, p in enumerate(plan_list):
		st.markdown(f"### Retrieved Plan {i+1}")
		st.markdown(f"- User Query: {p['user_query']}\n\n- Plan: {p['plan']}")
		st.write("====================")

st.title("Testing memory agent 🧠")

st.write("1. Initialize memory on planning")
st.write("For this portion, please upload a csv file with the following columns: User Query, Plan")
uploaded_file=st.file_uploader("Open your CSV file then click on the 'Upload' button", type="csv", key="initplanmemory")
if uploaded_file is not None:
    if st.button("Upload",key="intplanmemoryupload"):
        with st.spinner("Processing"):
            init_plan_memory(uploaded_file)
            st.success("Process done!")

st.write("2. Initialize memory on coding")
st.write("For this portion, please upload a csv file with the following columns: Category, Instruction, Code")
uploaded_file=st.file_uploader("Open your CSV file then click on the 'Upload' button", type="csv", key="initcodememory")
if uploaded_file is not None:
	if st.button("Upload", key="intcodememoryupload"):
		with st.spinner("Processing"):
			init_code_memory(uploaded_file)
			st.success("Process done!")

st.write("3. Retrieve plan from memory using user query")
user_query = st.text_area(f"Enter user query here:", key="getplan")
if st.button("Run", key="getplanbutton"):
	with st.spinner("Processing"):
		get_plan(user_query)
		st.success("Process done!")

st.write("4. Retrieve code from memory using instruction")
instruction = st.text_area(f"Enter instruction here:", key="getcode")
if st.button("Run", key="getcodebutton"):
	with st.spinner("Processing"):
		get_code(instruction)
		st.success("Process done!")

st.write("5. Add new plan to memory")
st.write("Yet to implement")

st.write("6. Add new code to memory")
st.write("Yet to implement")

st.write("====Old stuff====")
get = st.text_area(f"Get memory here:", key="get")
if st.button("Run", key="runget"):
	with st.spinner("Processing"):
		get_code(get)
		st.success("Process done!")

st.write("For this portion, please pass in a string consisting of user query, program code, category, compile, and ideal separated by ,")
add = st.text_area(f"Add a task to the memory here", key="add")
if st.button("Run", key="runadd"):
	with st.spinner("Processing"):
		add_code(add)
		st.success("Process done!")
		
