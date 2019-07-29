// Author: Long Qian
// Email: lqian8@jhu.edu

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;



public class UR5Controller : MonoBehaviour {


    /// <summary> 	
    /// TCPListener to listen for incomming TCP connection 	
    /// requests. 	
    /// </summary> 	
    private TcpListener tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread tcpListenerThread;
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;



    public GameObject RobotBase,port;
    public Button start;

    int Port = 0;
    private string[] buffer;
    public float[] jointValues = new float[6];
    private GameObject[] jointList = new GameObject[6];
    private float[] upperLimit = { 180f, 180f, 180f, 180f, 180f, 180f };
    private float[] lowerLimit = { -180f, -180f, -180f, -180f, -180f, -180f };

    bool run = false;



    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        run = !run;
        Debug.Log("clicked");
    }

    // Use this for initialization
    void Start () {
        initializeJoints();

      // var z= port.GetComponent<InputField>();
      // Port = int.Parse(z.text);
      
        // Start TcpServer background thread 		
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();

        start.onClick.AddListener(TaskOnClick);


    }

    // Update is called once per frame
    void LateUpdate () {

        //for (int i = 0; i < 6; i++)
        for ( int i = 0; i < 6; i ++) {
            Vector3 currentRotation = jointList[i].transform.localEulerAngles;
          //  Debug.Log(currentRotation);
            currentRotation.z = jointValues[i];
            jointList[i].transform.localEulerAngles = currentRotation;
           

        }
    }

    void OnGUI() {
        int boundary = 20;


#if UNITY_EDITOR
        int labelHeight = 25;
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 18;
#else
                int labelHeight = 40;
                GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 40;
#endif

        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        for (int i = 0; i < 6; i++)
        {
            GUI.Label(new Rect(50, boundary + (i * 2 + 1) * labelHeight+250, labelHeight * 7, labelHeight), "Joint " + i +" = "+jointValues[i].ToString("F1")+" "+ " ");
           // jointValues[i] = GUI.HorizontalSlider(new Rect(boundary+50 + labelHeight * 4, boundary + (i * 2 + 1) * labelHeight + labelHeight / 4, labelHeight * 5, labelHeight), jointValues[i], lowerLimit[i], upperLimit[i]);
        }
    }


    // Create the list of GameObjects that represent each joint of the robot
    void initializeJoints() {
        var RobotChildren = RobotBase.GetComponentsInChildren<Transform>();
        for (int i = 0; i < RobotChildren.Length; i++) {
            if (RobotChildren[i].name == "control0") {
                jointList[0] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "control1") {
                jointList[1] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "control2") {
                jointList[2] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "control3") {
                jointList[3] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "control4") {
                jointList[4] = RobotChildren[i].gameObject;
            }
            else if (RobotChildren[i].name == "control5") {
                jointList[5] = RobotChildren[i].gameObject;
            }
        }
    }

    /// <summary> 	
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
    /// </summary> 	
    private void ListenForIncommingRequests()
    {
        try
        {
            // Create listener on localhost port 8052. 			
            //tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052);
            tcpListener = new TcpListener(IPAddress.Any, 5000);
            tcpListener.Start();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message. 							
                            string clientMessage = Encoding.ASCII.GetString(incommingData, 0, incommingData.Length);
                            buffer = clientMessage.Split(';');

                            jointValues[0] = (float)(Convert.ToDouble(buffer[0]))*(-1);
                            jointValues[1] = (float)(Convert.ToDouble(buffer[1]))*(-1)-90f;
                            jointValues[2] = -1f*(float)(Convert.ToDouble(buffer[2]));
                            jointValues[3] = (float)(Convert.ToDouble(buffer[3])) * (-1) - 90f;
                            jointValues[4] = (float)(Convert.ToDouble(buffer[4]));
                            jointValues[5] = (float)(Convert.ToDouble(buffer[5]));


                            //Debug.Log("Angle 3" + jointValues[2]);
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
    /// <summary> 	
    /// Send message to client using socket connection. 	
    /// </summary> 	
    private void SendMessage()
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = "This is a message from your server.";
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }




}
