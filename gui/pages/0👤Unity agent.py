import streamlit as st
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def initialize_components():
    planner = A.Planner()
    coder = A.Coder()
    memorymanager = A.MemoryManager()
    # TODO: Critic
    return planner, coder, memorymanager

def generate_initial_script(instruction):
    planner, coder, memorymanager = initialize_components()
    plan = planner._generate_plan(instruction)
    plans = plan.split("\n")
    functions = []
    for plan in plans:
        functions.append(coder._generate_function(plan))
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