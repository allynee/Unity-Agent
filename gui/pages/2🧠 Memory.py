import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agents as A

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

def get_code(get):
	memorymanager = A.MemoryAgent()
	code_list = memorymanager._get_code(get)
	for i, c in enumerate(code_list):
		st.write(f"Code {i+1} is: {c}")

st.title("Testing memory agent")
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
		
