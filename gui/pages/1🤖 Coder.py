import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agents as A

def trigger_code_agent(task):
	coder = A.CodeAgent()
	output = coder._generate_code(task=task)
	st.write(output)

st.title("Testing code agent")
task = st.text_area(f"Enter simple task here", key="task")
if st.button("Run"):
	with st.spinner("Processing"):
		trigger_code_agent(task)
		st.success("Process done!")