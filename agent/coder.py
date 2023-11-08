import guidance
from dotenv import load_dotenv, find_dotenv
import os
import openai

class Coder:
    def __init__(self, model_name="gpt-3.5-turbo-1106", temperature=0, resume=False, ckpt_dir="ckpt", execution_error=True):
        load_dotenv(find_dotenv())
        openai.api_key = os.getenv("OPENAI_API_KEY")
        guidance.llm = guidance.llms.OpenAI(model_name)
        self.llm=guidance.llm
        self.ckpt_dir = ckpt_dir
        self.execution_error = execution_error
        #TODO: May need to account for resume, or not.
        #TODO: May need to account for execution error, or not

    def _generate_function(self, task, examples): 
        coder = guidance('''
        {{#system~}}
        You are an AI skilled in C# development for Unity's ubicomp space. 
        You will assist in creating functions as part of a larger system. 
        You will do so by translating pseudocode for a task to C# code.
        {{~/system}}
        {{#user~}}
        Your work revolves around our proprietary C# system. Our system comprises of: 
        - SceneAPI: This is the wrapper class for our ubicomp space. The methods here will allow for manipulating the whole space. 
        - Object3D: Each object in the space is of this type, and the methods here are available for every object. The anchor for the position and rotation of each object is at the bottom center of the object. 
        - Vector3D: Any 3-dimensional dataset uses this class to represent x, y, and z.
        - Color3D: Color information using RGBA.
                
        **As the script's class inherits from `SceneAPI`, you can directly call its methods without prefixing.**

        Use the provided system to manipulate the room.
                
        Follow these steps:
        1. Create a public void method that corresponds to the task. 
        2. Write a C# code that implements the task. You may use and create private fields and methods to achieve this. 
        3. Use Debug.Log statements for action logging and error handling.
        4. Adhere strictly to standard C# methods. 
                
        Here are all the classes and functions you may use in your code:
        ```
        Object types that can be created: (You must use the exact case-sensitive type of the object)
        Chair, Fox, Lamp, LED Cube, Push Button, Table, Vase, Zombie 

        namespace Enums
        {
            public enum WallName
            {
                Left,
                Right,
                BackLeft,
                BackRight,
            }
        }
        public class Object3D
        {
            public string GetType()
            public void WhenSelected(UnityAction<SelectEnterEventArgs> function)
            public void WhenNotSelected(UnityAction<SelectExitEventArgs> function)
            public void WhenHovered(UnityAction<HoverEnterEventArgs> function)
            public void WhenNotHovered(UnityAction<HoverExitEventArgs> function)
            public void SetColor(Color3D color)
            public void SetLuminousIntensity(float intensity)
            public void Illuminate(bool lit)
            public void SetPosition(Vector3D pos)
            public void SetRotation(Vector3D rot)
            public void SetSize(Vector3D s)
            public void SetSizeByScale(float s)
            public void Levitate(bool isLevitated)
            public bool IsLevitated()
            public Color GetColor()
            public float GetIntensity()
            public bool IsLit()
            public Vector3D GetPosition()
            public Vector3D GetRotation()
            public Vector3D GetSize()
            public GameObject ToGameObject()
        }

        public class SceneAPI
        {
            public Vector3D GetWallPosition(WallName wallname)
            public List<Object3D> GetAllObject3DsInScene()
            public Object3D FindObject3DByName(string objName)
            public bool IsObject3DInFieldOfView(Object3D obj)
            public List<Object3D> GetAllObject3DsInFieldOfView()
            public bool IsObjectTypeValid(string objectType)
            public List<string> GetAllValidObjectTypes()
            public Vector3D GetSceneSize()
            public Vector3D GetUserOrientation()
            public Vector3D GetUsersHeadPosition()
            public Vector3D GetUsersLeftHandPosition()
            public Vector3D GetUsersRightHandPosition()
            public Vector3D GetUsersLeftHandRotation()
            public Vector3D GetUsersRightHandRotation()
            public Vector3D GetUsersFeetPosition()
            public Object3D CreateObject(string newObjName, string objectType, Vector3D position, Vector3D rotation)
        }

        public class Vector3D
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
            Vector3D(float x, float y, float z)
            public Vector3D ToVector3()
            public Vector3D FromVector3(Vector3 vec)
        }

        public class Color3D
        {
            //All values below range from 0 to 1 
            public float r { get; set; }
            public float g { get; set; }
            public float b { get; set; }
            public float a { get; set; }
            Color3D(float r, float g, float b, float a)
        }
        ```
        The task to create a function for is: {{task}}. 
                                
        Here are examples of similar functions which may or may not be applicable to your task:\n {{examples}}
                
        Your format for responses should strictly be: 
        public void Method1()
        {
            // Insert the method code here
        }

        *Note: Output should not contain any text other than the method(s).*
        {{~/user}}
        {{#assistant~}}
        {{gen "function" temperature=0}}
        {{~/assistant}}
        ''')
        resp = coder(task=task, examples=examples)
        return resp["function"]
         
    def _generate_script(self, task, plan, functions):
        guidance.llm = guidance.llms.OpenAI("gpt-4")
        coder = guidance('''
        {{#system~}}
        You are a skilled C# developer tasked with crafting a coherent C# script, ensuring that created objects and states are managed and utilized effectively across multiple methods.
        {{~/system}}
        {{#user~}}
        User Task: {{task}}
        
        Actionable Tasks: {{plan}}
                                
        Function Templates: {{functions}}
                
        Follow these steps:
        1. Develop a public class inheriting from SceneAPI with a name relevant to the task context.
        2. Integrate and modify function templates to maintain script coherence.
        3. Use class-level variables to maintain state across methods.
        4. Use Debug.Log statements for action logging and error handling.
        5. Adhere strictly to standard C# methods and conventions.

        Your format for responses should strictly be: 
        ```
        public class YourChosenClassName : SceneAPI
        {	
            // Add any needed class members here
            
            private void Start()
                {
                    Method1();
                    Method2();
                    // And so on... The number of methods will depend on the user's request.
                }
            private void Update()
                {
                    // Insert the methods here if needed
                }
            public void Method1()
                {
                    // Insert the method code here
                }
            public void Method2()
                {
                    // Insert the method code here
                }
            // And so on... The number of methods will depend on the user's request. 
        }
        ```
        *Note: Output should not contain any text other than script containing method(s).*
        {{~/user}}
        {{#assistant~}}
        {{gen "script" temperature=0 max_tokens=4096}}
        {{~/assistant}}
        ''')  
        resp = coder(task=task, plan=plan, functions=functions)
        using = "using UnityEngine;\nusing UnityEngine.Events;\nusing UnityEngine.XR.Interaction.Toolkit;\nusing System;\nusing System.Collections.Generic;\nusing Enums;\n\n"
        return using + resp["script"]
    
#TODO: Add more imports for NavMeshAgent
             