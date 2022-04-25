Tech Demo Video: Youtube Link: 
https://youtu.be/mbbg262GHeg

Tech Demo Video: Google Drive:
https://drive.google.com/drive/folders/1u0BKPwHqN2B_59HP1CDNrJQdKo5SpD8I?usp=sharing

Tech Demo Google Drive Link:
https://drive.google.com/drive/folders/1u0BKPwHqN2B_59HP1CDNrJQdKo5SpD8I?usp=sharing

Tech Demo Github Link:
https://github.com/TarekQMUL/ECS7016-IAPCG-TECHDEMO
-----------------------------------------------------------------------------------------------------------------------
=======================================================================================================================
AIM: 

To Generate a map procedurally using Generative Grammars and to make agents try and trap the player

Generator:
Generative Grammars using L-Systems

Agent:

Combined Behaviours using Conditions
of Flocking,Evading,Pursuit and Wander
-----------------------------------------------------------------------------------------------------------------------
=======================================================================================================================
How to Use:
-----------------------------------------------------------------------------------------------------------------------
First Load the TechDemo Scene

Spawner:

Assign Agent to the obj parameter
and Player to the things to avoid Element 0
-----------------------------------------------------------------------------------------------------------------------
There is an Agent_Group this holds the agent and player:

You must assign the Agent to the Target Pred parameter of the player script on the inspector
You must assign the Player to the Target Prey and Target parameters of the agent script located in the inspector 
------------------------------------------------------------------------------------------------------------------------
There is a SandBox Group which is the arena
------------------------------------------------------------------------------------------------------------------------
There is a Generator_Group this holds all the GameObjects the Generator requires:

Assign the LSystemGenerator Script to the LsystemGen GameObject; this is where you can add the root sentence

Assign the LSystemDisplay Script to the Generator GameObject and assign LSystemGen, RoadCreator and BuildingCreator GameObjects 
to the corresponding parameters.

Assign the Road Prefabs to the corresponding parameters where the prefabs can be found in  thirdparty/AssetModels/Prefabs folder

Assign The buildings to the BuildingCreator GameObject which can be found in the Prefabs_Project folder:
Note: you should place the bigger buildings on the top and the smaller ones on the bottom 

-Size required is the size of the building length 
-Quantity is the number of buildings you want to place
-Quantity Already is the amount already placed which is 0 
-a Quanitity of -1 will fill out all spaces

Inside the Generator/Rules folder you can assign various rules for the Lsystem to work with

Scripts:(Generator)

- RoadCreator.cs - used to create the roads using dictionaries and hashsets and assigns correct direction for placement and hold the parameters to assign the prefabs
- BuildingCreator.cs - Used to place buildings in the correct orientation using dictionaries by finding out all the free plot spaces
- LSystemDisplay.cs - Used to draw the map onto the canvas and has the Action Letters Assignment
- BuildingTypes.cs - holds the prefabs of the buildings to be used
- Rule.cs - Creates scriptable objects and allows to randomize various rules 

Scripts:(Agent)

- AgentScript.cs - this is assigned to the Agent prefab and will wander 
  but when in  look range of player it will persuit and if within evade range the agent will evade the player

- PlayerScript.cs - this is assigned to the Player prefab and will wander but when in  look range of Agent it will persuit the Agent

------------------------------------------------------------------------------------------------------------------------------
==============================================================================================================================
REFERENCES:

The Generator was created by learning  how to use L Systems by following the Tutorial from Sunny Valley Studios on Youtube:
https://www.youtube.com/watch?v=OwuJ3pRzh-U&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=3

The Agent was Created by modifying the LAB 2 Steering behaviours Scripts 
to implement Wander, Flocking, Evade and Pursuit for the agent which changes the behaviour depending on how close the player gets

The Player was Also created by modifying the LAB 2 Steering behaviours Scripts 
to implement Wander,Pursuit.

Third Party Textures:
https://assetstore.unity.com/packages/2d/textures-materials/lowpoly-textures-pack-140717

Third Party Prefabs:
Quaternius-: https://quaternius.com/packs/modularstreets.html

Library:
Unity-Movement-Ai
https://github.com/sturdyspoon/unity-movement-ai
=======================================================================================================================
Tarek Ahmed 170431981