# UR-Simulation-in-Unity-Hardware-in-the-loop-Animation-of-Real-Robot-

**All the credit for original package and controller goes to Long Qian, Shuyang Chen for creating the prefab and initial script.

Simulates UR using joint angles being published by a real robot over TCP IP 

UR Simulator in Unity for replicating real UR with hardware in the loop

 
This fork contains following modifications

#### TCP server that can connect to real UR5/UR10
#### Some modifications in the joint angle mapping to replicate actual robot. 
#### Buttons for camera rotation.

You need to publish joint angles from UR script to this server in order to replicate motion.

Results on Editor:
![capture](UR.PNG "Capture in Unity3D Editor")
