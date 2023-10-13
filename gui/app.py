__version__ = "1.23.1"
app_name = "Unity Agent GUI ʕ •ᴥ•ʔ"

from dotenv import load_dotenv
import streamlit as st
st.set_page_config(layout='wide', page_icon="🦦", page_title=f'{app_name} {__version__}')
ss = st.session_state
if 'debug' not in ss: ss['debug'] = {}
header1 = st.empty() # for errors / messages
header2 = st.empty() # for errors / messages
header3 = st.empty() # for errors / messages

def main():
    load_dotenv()
    st.title("Unity Agent GUI 🦦")
    st.write("Head to the various pages to test different componenets separately.")
    
if __name__ == '__main__':
    main()

