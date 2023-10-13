# Unity-Agent

This is an individal WIP project for the module IS470 (Guided Research in Computing). 
The project topic is on lightweight Memory Management in Autonomous Code Agents

### About the project

🎯 Limitations in Conventional Approaches
- Emulating human coders through computational agents faces substantial challenges due to existing training methodologies.
- Traditional methods, like fine-tuning PLMs, demand considerable data and computational resource
- While LLMs like GPT-4 are capable of code generation, they face limitations in environments with limited or niche data (e.g., emerging languages, niche frameworks, and specialized tools).

🧐 Research Goal: Exploring adaptive learning as an alternative approach
- **Addressing Niche and Limited Data Challenges**: Develop a memory system that enables autonomous agents to work effectively in environments with constrained data.
- **Memory-Based Learning**: Implement a lightweight memory management system that empowers agents to store and retrieve insights from past experiences and tasks.
- **Enhanced Autonomy and Precision**: Enable the computational agents to autonomously refine and enhance their code generation strategies and outputs progressively.
- **Applicability in Diverse Programming Contexts**: Ensure the developed memory system is versatile, enabling agents to generalize from past learnings and acquire new capabilities for tackling diverse and challenging tasks within the same domain of problems.
  
🪑 Task: Embodied Prototyping in Ubicomp Environments
- The task entails enabling non-experts to design and prototype within ubiquitous computing (ubicomp) environments using intuitive interactions.
- It involves the development of an autonomous agent capable of translating natural language commands into executable code, subsequently modifying the virtual environment.

    **Example 1:** 
    - User command: “Put a button on the table”
    - Expected agent action: The agent manifests a button in the virtual space.
![Figure 1: The agent’s response to the command “Put a button on the table”.](/assets/example1.png)
  
    **Example 2:** 
    - User command: “When I press the button, make the table white”
    - Expected agent action: The table's color changes in real-time upon button press in the virtual space.
![Figure 2: Dynamic modification of virtual elements - making the table white upon pressing the button.](/assets/example2.png)

- The task faces challenges such as the acquisition of well-curated data due to the specialized and diverse nature of ubicomp spaces.
- The research aims to bypass the need for extensive curated data, enabling the code agent to incrementally learn from the environment and human feedback, enhancing its capabilities in modifying the virtual environment.

### Code Agent Memory Architecture
The introduced lightweight memory management mechanism for an autonomous code agent consists of four components: 
- **📑 Planner :** Interprets user commands into a sequence of instructions.
- **👾 Coder:** Translates instructions into executable code.
- **🧠 Memory Manager :** Provides the Planner and Coder with relevant past experiences.
- **🗣️ Critic:** Analyzes environmental data and user feedback to refine the agent’s output.

![Figure 3: Proposed architecture of the code agent.](/assets/architecture.png)

### Usage

### Future Work

### Acknowledgements

### License

### References
