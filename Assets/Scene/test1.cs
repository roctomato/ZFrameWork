using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class test1 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //if (Debug.isDebugBuild)
        //    Debug.Log("This is a debug build!");
        ZLog.D(this, "hello");
        ZLog.D(this, "hello message");
    }

    // Update is called once per frame
    void Update()
    {

    }

}