import chromadb
from chromadb.config import Settings
from dotenv import load_dotenv, find_dotenv
import guidance
from langchain.embeddings import OpenAIEmbeddings
import os
import openai
import sys
sys.path.append("/Users/allyne/Documents/GitHub/Unity-Agent/")
import utils as U

class MemoryAgent:
    def __init__(self, model_name="gpt-3.5-turbo", temperature=0, resume=False, retrieve_top_k=3, ckpt_dir="ckpt"):
        load_dotenv()
        load_dotenv(find_dotenv())
        openai.api_key = os.getenv("OPENAI_API_KEY")
        print("===============================")
        print(os.getenv("OPENAI_API_KEY"))
        print("===============================")
        guidance.llm = guidance.llms.OpenAI(model_name, temperature=temperature)
        self.llm=guidance.llm
        self.ckpt_dir = ckpt_dir
        self.retrieve_top_k = retrieve_top_k
        #TODO: May need to account for resume, or not. Not sure if need mkdir thingy too
        client = chromadb.Client(Settings(chroma_db_impl="duckdb+parquet",
                                persist_directory=f"../memory/{ckpt_dir}/vectordb"
                                ))
        self.vectordb =  client.get_or_create_collection(name="vectordb", embedding_function=OpenAIEmbeddings())
    
    def _get_code(self, task):
        print("Hello")
        embeddings = OpenAIEmbeddings()
        task_embedding = embeddings.embed_query(task)
        k = min(self.vectordb.count(), self.retrieve_top_k)
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

    def _add_new_code(self, info):
        p_description = info["p_name"]
        p_code = info["p_code"]
        p_category = info["p_category"]
        p_compile = info["p_compile"]
        p_ideal = info["p_ideal"]
        p_name = self._generate_program_name(p_description, p_code)

        embeddings = OpenAIEmbeddings()
        p_embedding = embeddings.embed_query(p_description)
        #TODO: Account for repeated programs/tasks
        self.vectordb.delete(ids=[p_name])
        self.vectordb.add(
            embeddings=[p_embedding],
            metadatas=[{ #TODO: May need to change categories accordingly
                "name": p_name,
                "category": p_category,
                "compile": p_compile,
                "ideal": p_ideal, 
            }],
            ids=[p_name] #TODO: Account for repeated tasks
        )
        U.dump_text(
            p_code, f"../memory/{self.ckpt_dir}/code/{p_name}.cs"
        )
        U.dump_text(
            p_description,
            f"../memory/{self.ckpt_dir}/description/{p_name}.txt",
        )
        return "Check folder."

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