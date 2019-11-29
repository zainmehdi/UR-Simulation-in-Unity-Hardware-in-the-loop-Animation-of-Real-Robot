// Author: Zain
// Email : zainmehdi31@gmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movement : MonoBehaviour
{


    public GameObject target;//the target object
    private float speedMod = 10.0f;//a speed modifier
    private Vector3 point;//the coord to the point where the camera looks at

    private bool rotateClock = false;
    private bool rotateAnticlock = false;

    public Button  rotateclockwise, rotateanticlockwise;
    void Start()
    {
        point = target.transform.position;
        //get target's coords
        //transform.LookAt(new Vector3(point.x,point.y,point.z+3));//makes the camera look to it
        rotateanticlockwise.onClick.AddListener(TaskOnRotateAntiClockwise);
        rotateclockwise.onClick.AddListener(TaskOnRotateClockwise);
    }

    private void TaskOnRotateClockwise()
    {

        rotateClock = !rotateClock;
      
    }

    private void TaskOnRotateAntiClockwise()
    {
        rotateAnticlock = !rotateAnticlock;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateClock)
        {
            transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 5 * Time.deltaTime * speedMod);
        }

        if (rotateAnticlock)
        {
            transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), -5 * Time.deltaTime * speedMod);
        }

    }
}

