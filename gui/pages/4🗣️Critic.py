import streamlit as st
from dotenv import load_dotenv
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import agent as A

def refine_plan(output_object):
    critic = A.Critic()
    output = critic._refine_plan(output_object)
    st.write(output)

