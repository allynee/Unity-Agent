import chromadb
from chromadb.config import Settings
import guidance
from langchain.embeddings import OpenAIEmbeddings
import os
from dotenv import load_dotenv

class MemoryManager:
    def __init__(self, model_name="gpt-4", temperature=0, resume=False, retrieve_top_k=3, ckpt_dir="ckpt"):
        load_dotenv()
        self.llm=guidance.llms.OpenAI(model_name)
        #TODO: Add utlity's f_mkdir for code, description, and vector db
        if resume:
            #TODO: Load skills
            pass
        else:
            self.skills = {}
            self.retrieve_top_k = retrieve_top_k
            client = chromadb.Client(Settings(chroma_db_impl="duckdb+parquet",
                                  persist_directory=f"{ckpt_dir}/vectordb"
                                    ))
        self.vectordb =  client.get_or_create_collection(name="vectordb", embedding_function=OpenAIEmbeddings())

    def _generate_code(): 
        pass

