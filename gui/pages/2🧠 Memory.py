import streamlit as st
import os
import sys
import pandas as pd
import shutil
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def init_plan_memory(uploaded_file):
	save_folder = "../init_memory"
	file_name = uploaded_file.name
	os.makedirs(save_folder, exist_ok=True)
	file_path = os.path.join(save_folder, file_name)
	with open(file_path, "wb") as f:
		f.write(uploaded_file.getbuffer())
	memorymanager = A.MemoryManager()
	output = memorymanager._init_plan_memory(file_path)
	st.write(output)

def init_code_memory(uploaded_file):
	save_folder = "../init_memory"
	file_name = uploaded_file.name
	os.makedirs(save_folder, exist_ok=True)
	file_path = os.path.join(save_folder, file_name)
	with open(file_path, "wb") as f:
		f.write(uploaded_file.getbuffer())
	memorymanager = A.MemoryManager()
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
	memorymanager = A.MemoryManager()
	output = memorymanager._add_new_code(info)
	st.write(output)

def get_code(instruction):
	memorymanager = A.MemoryManager()
	code_list = memorymanager._get_code(instruction)
	for i, c in enumerate(code_list):
		st.markdown(f"### Retrieved Code {i+1}")
		st.markdown(f"- Instruction: {c['instruction']}\n\n- Code:\n\n{c['code']}")
		st.write("====================")

def get_plan(user_query):
	memorymanager = A.MemoryManager()
	plan_list = memorymanager._get_plan(user_query)
	for i, p in enumerate(plan_list):
		st.markdown(f"### Retrieved Plan {i+1}")
		st.markdown(f"- User Query: {p['user_query']}\n\n- Plan: {p['plan']}")
		st.write("====================")

def delete_plan_memory():
	folders_to_clear = ["../memory/ckpt/plans"]
	files_to_clear = ["../init_memory/Plan.csv"]
	delete_files_and_folders(files_to_clear, folders_to_clear)
	memorymanager= A.MemoryManager()
	memorymanager._delete_plan_memory()
	st.write("Plan memory cleared!")

def delete_code_memory():
	folders_to_clear = ["../memory/ckpt/code"]
	files_to_clear = ["../init_memory/Code.csv"]
	delete_files_and_folders(files_to_clear, folders_to_clear)
	memorymanager= A.MemoryManager()
	memorymanager._delete_code_memory()
	st.write("Code memory cleared!")

def delete_files_and_folders(files_to_clear, folders_to_clear):
	for file_path in files_to_clear:
		try:
			os.remove(file_path)
			st.success(f"Deleted: {file_path}")
		except FileNotFoundError:
			st.warning(f"File Not Found: {file_path}")
		except Exception as e:
			st.error(f"Error: {str(e)}")

	for folder in folders_to_clear:
		try:
			shutil.rmtree(folder)
			st.success(f"Deleted all files in: {folder}")
		except FileNotFoundError:
			st.warning(f"Folder Not Found: {folder}")
		except Exception as e:
			st.error(f"Error: {str(e)}")

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

st.write("7. Delete plan memory")
if st.button("Delete", key="deleteplanbutton"):
	with st.spinner("Processing"):
		delete_plan_memory()
		st.success("Process done!")

st.write("8. Delete code memory")
if st.button("Delete", key="deletecodebutton"):
	with st.spinner("Processing"):
		delete_code_memory()
		st.success("Process done!")