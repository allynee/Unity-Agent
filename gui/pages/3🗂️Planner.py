import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def orchestrate(task):
	planner = A.Planner()
	output = planner._orchestrate(task)
	st.write(output)

def identify_task_type(task):
	planner = A.Planner()
	output = planner._identify_task_type(task)
	st.write(output)

def identify_prefab(task):
    planner = A.Planner()
    output = planner._identify_prefab(task)
    st.write(output)

def identify_location(task):
	planner = A.Planner()
	output = planner._identify_location(task)
	st.write(output)

def identify_create_object_operations(task):
	planner = A.Planner()
	output = planner._identify_create_object_operations(task)
	st.write(output)

def identify_modify_object_operations(task):
	planner = A.Planner()
	output = planner._identify_modify_object_operations(task)
	st.write(output)

# def identify_all_tasks(task):
# 	planner = A.PlannerAgent()
# 	output = planner._identify_all_tasks(task)
# 	st.write(output)


st.title("Testing planning agent")

st.write("1. Identify task type")
task3 = st.text_area(f"Identify task type from task here", key="task3")
if st.button("Run", key="run1"):
	with st.spinner("Processing"):
		identify_task_type(task3)
		st.success("Process done!")

st.write("2. Identify prefab to be created, modified, or deleted.")
st.write('All prefabs avail are: ["Chair", "TableLamp", "Table", "Screen", "Push Button"]')
task = st.text_area(f"Identify prefab from task here", key="task")
if st.button("Run", key="run2"):
	with st.spinner("Processing"):
		identify_prefab(task)
		st.success("Process done!")

st.write("3. Identify creation operations")
task5= st.text_area(f"Identify creation operations from task here", key="task5")
if st.button("Run", key="run5"):
	with st.spinner("Processing"):
		st.write("Location, orientation, size, color")
		identify_create_object_operations(task5)
		st.success("Process done!")

st.write("4. Identify modification operations")
task4 = st.text_area(f"Identify modification operations from task here", key="task4")
if st.button("Run", key="run4"):
	with st.spinner("Processing"):
		st.write("Location, orientation, size, color")
		identify_modify_object_operations(task4)
		st.success("Process done!")

st.write("5. Identify location specification")
st.write('Possible locations are: ["BackWall", "FrontWall", "LeftWall", "RightWall", "Floor", "Ceiling"]')
task2 = st.text_area(f"Identify location specification from task here", key="task2")
if st.button("Run", key="run3"):
	with st.spinner("Processing"):
		identify_location(task2)
		st.success("Process done!")

st.write("6. Orchestrate")
task1 = st.text_area(f"Create entire plan here", key="task1")
if st.button("Run", key="run6"):
	with st.spinner("Processing"):
		orchestrate(task1)
		st.success("Process done!")

# st.write("3. Identify number of elements")
# task3 = st.text_area(f"Identify number of elements from task here", key="task3")
# if st.button("Run", key="run3"):
# 	with st.spinner("Processing"):
# 		identify_number_of_elements(task3)
# 		st.success("Process done!")