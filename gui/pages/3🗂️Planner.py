import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agents as A

def identify_prefab(task):
    planner = A.PlannerAgent()
    output = planner._identify_prefab(task)
    st.write(output)

def identify_location(task):
	planner = A.PlannerAgent()
	output = planner._identify_location(task)
	st.write(output)

st.title("Testing planning agent")

st.write("1. Identify prefab to be created, modified, or deleted.")
st.write('All prefabs avail are: ["Chair", "TableLamp", "Table", "Screen", "Push Button"]')
task = st.text_area(f"Identify prefab from task here", key="task")
if st.button("Run", key="run1"):
	with st.spinner("Processing"):
		identify_prefab(task)
		st.success("Process done!")
		

st.write("2. Identify location specification")
st.write('Possible locations are: ["BackWall", "FrontWall", "LeftWall", "RightWall", "Floor", "Ceiling"]')
task2 = st.text_area(f"Identify location specification from task here", key="task2")
if st.button("Run", key="run2"):
	with st.spinner("Processing"):
		identify_location(task2)
		st.success("Process done!")