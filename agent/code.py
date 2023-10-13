import guidance
from dotenv import load_dotenv, find_dotenv
import os
import openai

class CodeAgent:
    def __init__(self, model_name="gpt-3.5-turbo", temperature=0, resume=False, ckpt_dir="ckpt", execution_error=True):
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
        You are a skilled C# developer tasked with manipulating a ubicomp space in Unity. You will do so by translating pseudocode for a task to C# code.
        {{~/system}}
        {{#user~}}
        Your work revolves around our proprietary C# system. Our system comprises of: 
        - SceneAPI: This is the wrapper class for our ubicomp space. The methods here will allow for manipulating the whole space. 
        - Object3D: Each object in the space is of this type, and the methods here are available for every object. The anchor for the position and rotation of each object is at the bottom center of the object. 
        - Vector3D: Any 3-dimentional dataset uses this class to represent x, y, and z.
        - Color3D: Color information using rgba.

        Use the provided system to manipulate the room.
        You must adhere strictly to standard C# methods. 
        Do not invoke methods outside of the ones provided. Although there might be resemblances to Unity, avoid Unity methods as our code isn't compatible with them. 
        
        Follow these steps:
        1. Create a "public void methodName()" method in your class that corresponds to the action with an appropriate name. The method may NOT accept or return any arguments and should result in a visible change in the 3d ubicomp space.
        2. Write a C# code that implements the task. You may use and create private fields and methods to achieve this. Stay within the boundaries of the given instructions, avoiding Unity-specific methods and extraneous code. To access the SceneAPI methods, use GetSceneAPI()
        
        Here are all the classes and functions you may use in your code:
        ```
        Object types that can be created: (You must use the exact case-sensitive type of the object)
        Cabinet,Ceiling LED Light,Chair,Lamp,LED Cube,Push Button,Standing Lamp,Table, Wall Fan
        namespace Enums
        {
            public enum WallName
            {
                Left,
                Right,
                BackLeft,
                BackRight,
            }

            public class MyEnums 
            {

            }
        }
        public class Object3D
        {
            public string GetType()
            public void setFanStatus(bool isFanOn)
            public bool IsFanOn()
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
        }
        public class SceneAPI
        {
            public Vector3D GetWallPosition(WallName wallname)
            public List<Object3D> GetAllObject3DsInScene()
            public Object3D FindObject3DByName(string objName)
            public bool IsObject3DInFieldOfView(Object3D obj)
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
            public List<Object3D> GetAllObject3DsInFieldOfView()
            public void CeilingLightSwitch()
            public void StandingLightSwitch()
        }
        public class Vector3D
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
            Vector3D(float x, float y, float z)
        }
        public class Color3D
        {
            public float r { get; set; }// The value ranges from 0 to 1
            public float g { get; set; }// The value ranges from 0 to 1
            public float b { get; set; }// The value ranges from 0 to 1
            public float a { get; set; }// The value ranges from 0 to 1
            Color3D(float r, float g, float b, float a)
        }

        The task you have to create a function for is: {{task}}. 
                         
        Here is an example of similar functions that worked:\n {{examples}}
        
        Your output should strictly follow the format below. Do NOT include any other information in your output.
        public void Method1()
    	{
        	// Insert the method code here
    	}
        {{~/user}}
        {{#assistant~}}
        {{gen "function" temperature=0}}
        {{~/assistant}}
        ''')
        resp = coder(task=task, examples=examples)
        return resp["function"]
    
    def _generate_script(self, task, plan, functions):
        guidance.llm = guidance.llms.OpenAI("gpt-3.5-turbo-16k")
        coder = guidance('''
        {{#system~}}
        You are a skilled C# developer tasked with crafting a coherent C# script, ensuring that created objects and states are managed and utilized effectively across multiple methods.
        {{~/system}}
        {{#user~}}
        User Task: {{task}}
        
        Actionable Tasks: {{plan}}
                         
        Function Templates: {{functions}}
        
        Pay close attention to the following guidelines:
        - Formulate a "public class [classname] : SolutionClass" where '[classname]' should represent the user's task context.
        - Utilize supplied function templates for actionable tasks.
        - Allow slight modifications to function templates to maintain logical coherence, and integrate necessary private members or methods.
        - Abide by standard C# methods and conventions, ensuring you do not use Unity-specific or unrelated external methods.
        
        It's paramount that your script maintains a coherent state. Specifically:
        - If an object is created or modified in one method, ensure that subsequent methods act on the same object instead of creating new ones.
        - If a method depends on the result of another, ensure the ordering and parameterization of method calls adhere to logical and functional consistency.
        - Leverage class-level variables to maintain state and object references across multiple method calls.
        
        Your output should strictly follow the format below. Do NOT include any other information in your output.
        ```
        using UnityEngine;
        using UnityEngine.Events;
        using UnityEngine.XR.Interaction.Toolkit;
        using System;
        using System.Collections.Generic;
                         
        public class YourChosenClassName : SolutionClass 
        {	
                // Add any needed class members here
            
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
        {{~/user}}
        {{#assistant~}}
        {{gen "script" temperature=0 max_tokens=3200}}
        {{~/assistant}}
        ''')  
        resp = coder(task=task, plan=plan, functions=functions)
        return resp["script"]
             