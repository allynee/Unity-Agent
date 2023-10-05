import chromadb
from chromadb.config import Settings
import csv
from dotenv import load_dotenv, find_dotenv
import guidance
from langchain.embeddings import OpenAIEmbeddings
import os
import openai
from time import time as now
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import utils as U

class MemoryAgent:
    def __init__(self, model_name="gpt-3.5-turbo", temperature=0, resume=False, retrieve_top_k=3, ckpt_dir="ckpt"):
        load_dotenv(find_dotenv())
        openai.api_key = os.getenv("OPENAI_API_KEY")

        guidance.llm = guidance.llms.OpenAI(model_name, temperature=temperature)
        self.llm=guidance.llm

        self.ckpt_dir = ckpt_dir
        self.retrieve_top_k = retrieve_top_k

        #TODO: May need to account for resume, or not. Not sure if need mkdir thingy too
        settings = Settings(chroma_db_impl="duckdb+parquet",
                                     persist_directory=f"../memory/{ckpt_dir}")
        client = chromadb.Client(settings)

        self.embeddings = OpenAIEmbeddings()

        self.plansdb = client.get_or_create_collection(name="plansdb", embedding_function=self.embeddings)
        self.codedb = client.get_or_create_collection(name="codedb", embedding_function=self.embeddings)
    
    def _init_plan_memory(self, csv_path):
        t0=now()
        with open(csv_path, "r") as file:
            reader = csv.DictReader(file)
            for i, row in enumerate(reader):
                print(f"Embedding plan {i+1}...")
                user_query = row["User Query"]
                plan = row["Plan"]
                user_query_embedding = self.embeddings.embed_query(user_query)
                self.plansdb.add(
                    embeddings=[user_query_embedding],
                    metadatas=[{
                        "user_query": user_query,
                        "plan": plan,
                        }],
                    ids=[user_query]
                )
                U.dump_text(
                    f"User query:\n{user_query}\n\nPlan:\n{plan}", f"../memory/{self.ckpt_dir}/plans/{user_query}.txt"
                )
        return f"Intialized memory on planning in {now()-t0} seconds."
    
    def _init_code_memory(self, csv_path):
        t0=now()
        with open(csv_path, "r") as file:
            reader = csv.DictReader(file)
            for i, row in enumerate(reader):
                print(f"Embedding code {i+1}...")
                category = row["Category"]
                instruction = row["Instruction"]
                code = row["Code"]
                instruction_embedding = self.embeddings.embed_query(instruction)
                self.codedb.add(
                    embeddings=[instruction_embedding],
                    metadatas=[{
                        "category": category,
                        "instruction": instruction,
                        "code": code,
                        }],
                    ids=[instruction]
                )
                U.dump_text(
                    f"Instruction:\n{instruction}\n\nCategory:\n{category}\n\nCode:\n{code}", f"../memory/{self.ckpt_dir}/code/{category} - {instruction}.txt"
                )
        return f"Intialized memory on coding in {now()-t0} seconds."
    
    def _get_code(self, instruction):
        instruction_embedding = self.embeddings.embed_query(instruction)
        k = min(self.codedb.count(), self.retrieve_top_k)
        if k==0:
            return []
        print(f"Retrieving {k} codes...")
        codes = self.codedb.query(
            query_embeddings=instruction_embedding,
			n_results=k,
			#where={"metadata_field": "is_equal_to_this"}, #TODO: Potentially filter by category
            #where_document={"$contains":"search_string"}
			include=["metadatas"]
        )
        return codes["metadatas"][0] 

    def _get_plan(self, user_query):
        user_query_embedding = self.embeddings.embed_query(user_query)
        k = min(self.plansdb.count(), self.retrieve_top_k)
        if k==0:
            return []
        print(f"Retrieving {k} plans...")
        plans = self.plansdb.query(
            query_embeddings=user_query_embedding,
            n_results=k,
            include=["metadatas"]
        )
        return plans["metadatas"][0]

    def _add_new_code(self, info):
        category = info["category"]
        instruction = info["instruction"]
        code = info["code"]
        instruction_embedding = self.embeddings.embed_query(instruction)
        self.codedb.add(
            embeddings=[instruction_embedding],
            metadatas=[{
                "category": category,
                "instruction": instruction,
                "code": code,
                }],
            ids=[instruction] #TODO: Account for repeated instructions
        )
        U.dump_text(
            f"Instruction:\n{instruction}\n\nCategory:\n{category}\n\nCode:\n{code}", f"../memory/{self.ckpt_dir}/code/{category} - {instruction}.txt"
        )
        return f"Added code for instruction \"{instruction}\""
    
    def _add_new_plan(self, info):
        user_query = info["user_query"]
        plan = info["plan"]
        user_query_embedding = self.embeddings.embed_query(user_query)
        self.plansdb.add(
            embeddings=[user_query_embedding],
            metadatas=[{
                "user_query": user_query,
                "plan": plan,
                }],
            ids=[user_query] #TODO: Account for repeated user queries
        )
        U.dump_text(
            f"User query:\n{user_query}\n\nPlan:\n{plan}", f"../memory/{self.ckpt_dir}/plans/{user_query}.txt"
        )
        return f"Added plan for user query \"{user_query}\""
    
    #OLD CODE#

    def _generate_program_name(self, program_description, program_code):
        #TODO: Add example for generating descriptions
        description_generator = guidance('''
        {{#system~}}
        You are a helpful assistant that generates a terse program name of for a piece of Unity C# code. 
        {{~/system}}
        {{#user~}}               
        The program description is {{program_description}}
        The code is {{program_code}}
        {{~/user}}
        {{#assistant~}}
        {{gen "description" temperature=0}}
        {{~/assistant}}
        ''')
        resp = description_generator(program_description=program_description, program_code=program_code)
        return resp["description"]

    def _add_new_strategy(self):
        pass


#get_code if following old vibes
'''
    def get_code(self, task):
        task_embedding = OpenAIEmbeddings().embed_query(task)
        k = min(self.vectordb._collection.count(), self.retrieve_top_k)
        if k==0:
            return []
        print(f"Retrieving {k} code chunks")
        code_chunks = self.vectordb.query(
            query_embeddings=task_embedding,
			n_results=k,
			#where={"metadata_field": "is_equal_to_this"}, #TODO: Filtering by subtasks 
            #where_document={"$contains":"search_string"}
			include=["metadatas"]
        )
        return code_chunks["metadatas"][0] #TODO: Not sure if this syntax is correct
'''

#old instructions for generating prog description
'''
        1) Do not mention the function name.
        2) There might be some helper functions before the main function, but you only need to describe the main function.
        3) Try to summarize the function in no more than 6 sentences.
        4) Your response should be a single line of text.
        5) If the content is not related to unity, return "NA"
'''