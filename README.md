# UR-Simulation-in-Unity-Hardware-in-the-loop-Animation-of-Real-Robot-
Simulates UR using joint angles being published by a real robot over TCP IP 

UR Simulator in Unity for replicating real UR with hardware in the loop

All the credit for original package and controller goes to Long Qian, Shuyang Chen for creating the prefab and initial script. 
This fork contains a TCP server that can connect to real UR5/UR10 and some modifications in the joint angle mapping to replicate actual robot. Moreover GUI has been added and extended to allow for camera rotation.
You need to publish joint angles from UR script to this server in order to replicate motion.

Results on Editor:
![capture](UR.png "Capture in Unity3D Editor")
